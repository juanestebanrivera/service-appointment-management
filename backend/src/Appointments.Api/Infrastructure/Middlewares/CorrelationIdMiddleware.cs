namespace Appointments.Api.Infrastructure.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private const string CorrelationIdHeaderKey = "X-Correlation-ID";

    private readonly ILogger<CorrelationIdMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        string correlationId =
            context.Request.Headers.TryGetValue(CorrelationIdHeaderKey, out var headerValue) &&
            !string.IsNullOrWhiteSpace(headerValue)
                ? headerValue.ToString()
                : Guid.NewGuid().ToString();

        context.Response.Headers[CorrelationIdHeaderKey] = correlationId;

        using (_logger.BeginScope(new { CorrelationId = correlationId }))
        {
            await next(context);
        }
    }
}