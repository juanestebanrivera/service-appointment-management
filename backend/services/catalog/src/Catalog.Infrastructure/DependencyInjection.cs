using Catalog.Application.Abstractions;
using Catalog.Domain.Establishments;
using Catalog.Domain.Services;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Persistence.Establishments;
using Catalog.Infrastructure.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

        services.AddDbContext<CatalogDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<CatalogDbContext>());
        services.AddScoped<IEstablishmentRepository, EstablishmentRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();

        return services;
    }
}