using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Appointments.Api.Shared.Filters.Idempotency;

public class IdempotencyFilter(IDistributedCache cache, int expirationMinutes = 60) : IEndpointFilter
{
    private const string HeaderKey = "X-Idempotency-Key";
    private readonly IDistributedCache _cache = cache;
    private readonly int _expirationMinutes = expirationMinutes;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (!TryGetIdempotencyKey(context, out var idempotencyKey))
            return CreateProblem(StatusCodes.Status400BadRequest);

        var cancellationToken = context.HttpContext.RequestAborted;
        var cacheKey = $"idempotency:{idempotencyKey}";
        var cachedResult = await _cache.GetStringAsync(cacheKey, cancellationToken);

        if (cachedResult is not null)
            return ReturnCachedResponse(context, cachedResult);

        var inProgressResponse = new IdempotentResponse(IdempotencyStatus.InProgress);
        await SaveInCacheAsync(cacheKey, inProgressResponse, TimeSpan.FromSeconds(30), cancellationToken);

        try
        {
            cancellationToken.ThrowIfCancellationRequested();

            var result = await next(context);

            if (cancellationToken.IsCancellationRequested)
            {
                await _cache.RemoveAsync(cacheKey, cancellationToken);
                return CreateProblem(StatusCodes.Status499ClientClosedRequest);
            }

            bool isSuccess = false;
            int? statusCode = null;

            if (result is IStatusCodeHttpResult httpResult)
            {
                statusCode = httpResult.StatusCode;
                isSuccess = statusCode is >= 200 and < 300;
            }

            if (!isSuccess)
            {
                await _cache.RemoveAsync(cacheKey, cancellationToken);
                return result;
            }

            var body = result is IValueHttpResult valueHttpResult ? valueHttpResult.Value : null;


            return new CachingResult(
                result is IResult inner ? inner : CreateProblem(StatusCodes.Status500InternalServerError),
                async location =>
                {
                    var headers = new Dictionary<string, string>();

                    if (!string.IsNullOrEmpty(location))
                    {
                        headers.Add("Location", location);
                    }

                    var completedResponse = new IdempotentResponse(IdempotencyStatus.Completed, statusCode ?? StatusCodes.Status200OK, body, headers);

                    await SaveInCacheAsync(cacheKey, completedResponse, TimeSpan.FromMinutes(_expirationMinutes), cancellationToken);
                }
            );
        }
        catch (OperationCanceledException)
        {
            await _cache.RemoveAsync(cacheKey);
            return CreateProblem(StatusCodes.Status499ClientClosedRequest);
        }
        catch (Exception)
        {
            await _cache.RemoveAsync(cacheKey);
            throw;
        }
    }

    private static bool TryGetIdempotencyKey(EndpointFilterInvocationContext context, out Guid idempotencyKey)
    {
        idempotencyKey = Guid.Empty;

        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderKey, out var idempotencyHeader))
            return false;

        if (!Guid.TryParse(idempotencyHeader, out idempotencyKey))
            return false;

        if (idempotencyKey == Guid.Empty)
            return false;

        return true;
    }

    private static IResult CreateProblem(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest => Results.Problem(
                title: "Invalid Idempotency Key",
                statusCode: statusCode,
                detail: $"The '{HeaderKey}' header is missing or invalid. Please provide a valid GUID."
            ),
            StatusCodes.Status409Conflict => Results.Problem(
                title: "Request In Progress",
                statusCode: statusCode,
                detail: "A request is currently being processed. Please wait and try again later."
            ),
            StatusCodes.Status499ClientClosedRequest => Results.Problem(
                title: "Request Cancelled",
                statusCode: statusCode,
                detail: "The request was cancelled by the client."
            ),
            _ => Results.Problem(
                title: "Unexpected Error",
                statusCode: statusCode,
                detail: "An unexpected error occurred while processing the idempotent request."
            )
        };
    }

    private static IResult ReturnCachedResponse(EndpointFilterInvocationContext context, string cachedResult)
    {
        var response = JsonSerializer.Deserialize<IdempotentResponse>(cachedResult, JsonSerializerOptions.Web);

        if (response is null)
            return CreateProblem(StatusCodes.Status500InternalServerError);

        if (response.Status == IdempotencyStatus.InProgress)
            return CreateProblem(StatusCodes.Status409Conflict);

        foreach (var header in response.Headers ?? Enumerable.Empty<KeyValuePair<string, string>>())
        {
            context.HttpContext.Response.Headers[header.Key] = header.Value;
        }

        return Results.Json(response.Body, statusCode: response.StatusCode ?? StatusCodes.Status500InternalServerError);
    }

    private async Task SaveInCacheAsync(string cacheKey, IdempotentResponse response, TimeSpan expiration, CancellationToken cancellationToken = default)
    {
        var serializedResponse = JsonSerializer.Serialize(response, JsonSerializerOptions.Web);
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await _cache.SetStringAsync(
            cacheKey,
            serializedResponse,
            cacheOptions,
            cancellationToken
        );
    }

    private sealed class CachingResult(IResult inner, Func<string?, Task> onExecuted) : IResult
    {
        public async Task ExecuteAsync(HttpContext httpContext)
        {
            await inner.ExecuteAsync(httpContext);

            var location = httpContext.Response.Headers.Location.ToString();
            await onExecuted(string.IsNullOrEmpty(location) ? null : location);
        }
    }
}