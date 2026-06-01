using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Catalog.Api.Infrastructure.Endpoints;

public static class EndpointExtension
{
    public static IServiceCollection AddEndpoints(this IServiceCollection services)
    {
        var assembly = typeof(Program).Assembly;
        var endpointTypes = assembly.DefinedTypes
            .Where(type => type.IsAssignableTo(typeof(IEndpoint))
                           && type is { IsClass: true, IsAbstract: false, IsInterface: false });

        var serviceDescriptors = endpointTypes
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type));

        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }

    public static void MapApiEndpoints(this WebApplication app)
    {
        var apiVersionSet = app.NewApiVersionSet()
                               .HasApiVersion(new ApiVersion(1))
                               .ReportApiVersions()
                               .Build();

        var globalGroup = app.MapGroup("/api/v{version:apiVersion}")
                             .WithApiVersionSet(apiVersionSet);

        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoints(globalGroup);
        }
    }
}