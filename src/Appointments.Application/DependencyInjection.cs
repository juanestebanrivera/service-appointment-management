using Appointments.Application.Interfaces.Services;
using Appointments.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<IAppointmentService, AppointmentService>();

        return services;
    }
}