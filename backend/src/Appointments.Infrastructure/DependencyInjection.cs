using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.Services;
using Appointments.Domain.Users;
using Appointments.Infrastructure.Authentication;
using Appointments.Infrastructure.Persistence;
using Appointments.Infrastructure.Persistence.Appointments;
using Appointments.Infrastructure.Persistence.Clients;
using Appointments.Infrastructure.Persistence.Services;
using Appointments.Infrastructure.Persistence.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ClientRepository>();
        services.AddScoped<IClientRepository>(sp => sp.GetRequiredService<ClientRepository>());
        services.AddScoped<IQueryableRepository<Client>>(sp => sp.GetRequiredService<ClientRepository>());

        services.AddScoped<ServiceRepository>();
        services.AddScoped<IServiceRepository>(sp => sp.GetRequiredService<ServiceRepository>());
        services.AddScoped<IQueryableRepository<Service>>(sp => sp.GetRequiredService<ServiceRepository>());

        services.AddScoped<AppointmentRepository>();
        services.AddScoped<IAppointmentRepository>(sp => sp.GetRequiredService<AppointmentRepository>());
        services.AddScoped<IQueryableRepository<Appointment>>(sp => sp.GetRequiredService<AppointmentRepository>());

        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }

    public static async Task ApplyMigrationsAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await db.Database.MigrateAsync();
    }
}