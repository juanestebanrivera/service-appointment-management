using Appointments.Application.Features.Appointments.Commands.BookAppointment;
using Appointments.Application.Features.Appointments.Commands.CancelAppointment;
using Appointments.Application.Features.Appointments.Commands.CompleteAppointment;
using Appointments.Application.Features.Appointments.Commands.ConfirmAppointment;
using Appointments.Application.Features.Appointments.Commands.DeleteAppointment;
using Appointments.Application.Features.Appointments.Commands.MarkAppointmentAsNoShow;
using Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentById;
using Appointments.Application.Features.Clients.Commands.CreateClient;
using Appointments.Application.Features.Clients.Commands.DeleteClient;
using Appointments.Application.Features.Clients.Commands.UpdateClient;
using Appointments.Application.Features.Clients.Queries.GetAllClients;
using Appointments.Application.Features.Clients.Queries.GetClientById;
using Appointments.Application.Features.Services.Commands.CreateService;
using Appointments.Application.Features.Services.Commands.DeleteService;
using Appointments.Application.Features.Services.Commands.UpdateService;
using Appointments.Application.Features.Services.Queries.GetAllServices;
using Appointments.Application.Features.Services.Queries.GetServiceById;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Clients
        services.AddScoped<ICreateClientCommandHandler, CreateClientCommandHandler>();
        services.AddScoped<IUpdateClientCommandHandler, UpdateClientCommandHandler>();
        services.AddScoped<IDeleteClientCommandHandler, DeleteClientCommandHandler>();
        services.AddScoped<IGetAllClientsQueryHandler, GetAllClientsQueryHandler>();
        services.AddScoped<IGetClientByIdQueryHandler, GetClientByIdQueryHandler>();

        // Services
        services.AddScoped<ICreateServiceCommandHandler, CreateServiceCommandHandler>();
        services.AddScoped<IUpdateServiceCommandHandler, UpdateServiceCommandHandler>();
        services.AddScoped<IDeleteServiceCommandHandler, DeleteServiceCommandHandler>();
        services.AddScoped<IGetAllServicesQueryHandler, GetAllServicesQueryHandler>();
        services.AddScoped<IGetServiceByIdQueryHandler, GetServiceByIdQueryHandler>();

        // Appointments
        services.AddScoped<IBookAppointmentCommandHandler, BookAppointmentCommandHandler>();
        services.AddScoped<IRescheduleAppointmentCommandHandler, RescheduleAppointmentCommandHandler>();
        services.AddScoped<IConfirmAppointmentCommandHandler, ConfirmAppointmentCommandHandler>();
        services.AddScoped<ICancelAppointmentCommandHandler, CancelAppointmentCommandHandler>();
        services.AddScoped<ICompleteAppointmentCommandHandler, CompleteAppointmentCommandHandler>();
        services.AddScoped<IMarkAppointmentAsNoShowCommandHandler, MarkAppointmentAsNoShowCommandHandler>();
        services.AddScoped<IDeleteAppointmentCommandHandler, DeleteAppointmentCommandHandler>();
        services.AddScoped<IGetAllAppointmentsQueryHandler, GetAllAppointmentsQueryHandler>();
        services.AddScoped<IGetAppointmentByIdQueryHandler, GetAppointmentByIdQueryHandler>();

        return services;
    }
}