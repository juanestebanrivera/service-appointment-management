using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.Services;
using Appointments.Infrastructure.Persistence;
using Appointments.Infrastructure.Persistence.Appointments;
using Appointments.Infrastructure.Persistence.Clients;
using Appointments.Infrastructure.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IClientRepository, ClientRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        return services;
    }
}