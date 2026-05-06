using System.Threading.RateLimiting;

namespace Appointments.Api.Infrastructure.RateLimiting;

public static class RateLimiterExtensions
{
    public static IServiceCollection AddCustomRateLimiting(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RateLimitOptions>(configuration.GetSection(RateLimitOptions.SectionName));

        var rateLimitSettings = configuration
            .GetSection(RateLimitOptions.SectionName)
            .Get<RateLimitOptions>() ?? new RateLimitOptions();

        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out TimeSpan retryAfterTimeSpan))
                {
                    var retryAfterSeconds = (int)Math.Ceiling(retryAfterTimeSpan.TotalSeconds);
                    context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds.ToString();
                }

                var problemResult = GetRejectedProblemResult();

                await problemResult.ExecuteAsync(context.HttpContext);
            };

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                return RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = rateLimitSettings.Global.PermitLimit,
                        Window = TimeSpan.FromSeconds(rateLimitSettings.Global.WindowInSeconds),
                        SegmentsPerWindow = rateLimitSettings.Global.SegmentsPerWindow,
                        QueueLimit = rateLimitSettings.Global.QueueLimit
                    }
                );
            });
        });

        return services;
    }

    private static IResult GetRejectedProblemResult()
    {
        return Results.Problem(
            title: "Too Many Requests",
            statusCode: StatusCodes.Status429TooManyRequests,
            detail: "You have exceeded the allowed number of requests. Please try again later."
        );
    }
}