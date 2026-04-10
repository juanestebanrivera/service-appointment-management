using Appointments.Api.Abstractions;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Appointments.Api.Extensions;

public static class MapEndpointsExtension
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

    public static IApplicationBuilder RegisterEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoints(app);
        }
        
        return app;
    }
}