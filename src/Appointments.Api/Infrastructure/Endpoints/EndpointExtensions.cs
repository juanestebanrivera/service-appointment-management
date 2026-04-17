using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Appointments.Api.Infrastructure.Endpoints;

public static class EndpointExtensions
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;

        var endpointTypes = assembly.DefinedTypes
            .Where(type => type.IsAssignableTo(typeof(IEndpoint)) &&
                           type is { IsClass: true, IsAbstract: false, IsInterface: false });

        var serviceDescriptors = endpointTypes
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }

    public static IApplicationBuilder MapApiEndpoints(this WebApplication app)
    {
        var apiVersionSet = app.NewApiVersionSet()
            .HasApiVersion(new(1))
            .ReportApiVersions()
            .Build();

        var globalApiGroup = app.MapGroup("/api/v{version:apiVersion}")
                                .WithApiVersionSet(apiVersionSet)
                                .ProducesProblem(StatusCodes.Status429TooManyRequests)
                                .ProducesProblem(StatusCodes.Status500InternalServerError);

        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoints(globalApiGroup);
        }

        return app;
    }
}