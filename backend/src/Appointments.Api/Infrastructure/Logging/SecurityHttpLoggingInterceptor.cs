using Microsoft.AspNetCore.HttpLogging;

namespace Appointments.Api.Infrastructure.Logging;

public class SecurityHttpLoggingInterceptor : IHttpLoggingInterceptor
{
    public ValueTask OnRequestAsync(HttpLoggingInterceptorContext logContext)
    {
        if (logContext.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            logContext.AddParameter("Authorization", "[REDACTED]");
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask OnResponseAsync(HttpLoggingInterceptorContext logContext)
    {
        return ValueTask.CompletedTask;
    }
}