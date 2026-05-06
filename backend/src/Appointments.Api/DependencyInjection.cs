using Appointments.Api.Infrastructure.Authentication;
using Appointments.Api.Infrastructure.Endpoints;
using Appointments.Api.Infrastructure.Logging;
using Appointments.Api.Infrastructure.Middlewares;
using Appointments.Api.Infrastructure.RateLimiting;
using Asp.Versioning;

namespace Appointments.Api;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPresentation(WebApplicationBuilder builder)
        {
            services.AddApiVersion();
            services.AddEndpoints();
            services.AddOpenApi();

            services.AddCustomRateLimiting(builder.Configuration);
            services.AddCustomHttpLogging(builder.Environment);
            services.AddCaching();

            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails(options =>
            {
                options.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                    context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                };
            });

            services.AddJwtAuthentication(builder.Configuration);

            return services;
        }

        private IServiceCollection AddApiVersion()
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new(1);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        private IServiceCollection AddCaching()
        {
            services.AddOutputCache(options =>
            {
                options.DefaultExpirationTimeSpan = TimeSpan.FromSeconds(60);
                options.MaximumBodySize = 1024 * 1024; // 1 MB
            });

            // TODO: Use Redis or another distributed cache.
            services.AddDistributedMemoryCache();

            return services;
        }
    }
}