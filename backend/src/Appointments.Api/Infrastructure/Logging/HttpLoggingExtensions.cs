using Microsoft.AspNetCore.HttpLogging;

namespace Appointments.Api.Infrastructure.Logging;

public static class HttpLoggingExtensions
{
    public static IServiceCollection AddCustomHttpLogging(this IServiceCollection services, IWebHostEnvironment environment)
    {
        services.AddHttpLogging(options =>
        {
            options.LoggingFields =
                HttpLoggingFields.RequestMethod |
                HttpLoggingFields.RequestPath |
                HttpLoggingFields.RequestQuery |
                HttpLoggingFields.RequestHeaders |
                HttpLoggingFields.ResponseStatusCode |
                HttpLoggingFields.ResponseHeaders |
                HttpLoggingFields.Duration;

            options.RequestHeaders.Add("User-Agent");
            options.RequestHeaders.Add("X-Correlation-ID");

            options.RequestBodyLogLimit = 2048; // 2 KB
            options.ResponseBodyLogLimit = 2048; // 2 KB

            if (environment.IsDevelopment())
            {
                options.LoggingFields |= HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody;
                options.RequestBodyLogLimit = 1024 * 32; // 32 KB
                options.ResponseBodyLogLimit = 1024 * 32; // 32 KB
            }
        });

        services.AddHttpLoggingInterceptor<SecurityHttpLoggingInterceptor>();

        return services;
    }
}