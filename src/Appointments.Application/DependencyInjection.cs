using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Appointments.Commands.BookAppointment;
using Appointments.Application.Features.Appointments.Commands.CancelAppointment;
using Appointments.Application.Features.Appointments.Commands.CompleteAppointment;
using Appointments.Application.Features.Appointments.Commands.ConfirmAppointment;
using Appointments.Application.Features.Appointments.Commands.MarkAppointmentAsNoShow;
using Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentById;
using Appointments.Application.Features.Clients;
using Appointments.Application.Features.Clients.Commands.CreateClient;
using Appointments.Application.Features.Clients.Commands.DeleteClient;
using Appointments.Application.Features.Clients.Commands.UpdateClient;
using Appointments.Application.Features.Clients.Queries.GetAllClients;
using Appointments.Application.Features.Clients.Queries.GetClientById;
using Appointments.Application.Features.Services;
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
        services.AddSingleton(TimeProvider.System);

        // Clients
        services.AddScoped<ICommandHandler<CreateClientCommand, Guid>, CreateClientCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateClientCommand>, UpdateClientCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteClientCommand>, DeleteClientCommandHandler>();
        services.AddScoped<IQueryHandler<GetAllClientsQuery, IEnumerable<ClientResponse>>, GetAllClientsQueryHandler>();
        services.AddScoped<IQueryHandler<GetClientByIdQuery, ClientResponse>, GetClientByIdQueryHandler>();
        
        // Services
        services.AddScoped<ICommandHandler<CreateServiceCommand, Guid>, CreateServiceCommandHandler>();
        services.AddScoped<ICommandHandler<UpdateServiceCommand>, UpdateServiceCommandHandler>();
        services.AddScoped<ICommandHandler<DeleteServiceCommand>, DeleteServiceCommandHandler>();
        services.AddScoped<IQueryHandler<GetAllServicesQuery, IEnumerable<ServiceResponse>>, GetAllServicesQueryHandler>();
        services.AddScoped<IQueryHandler<GetServiceByIdQuery, ServiceResponse>, GetServiceByIdQueryHandler>();

        // Appointments
        services.AddScoped<ICommandHandler<BookAppointmentCommand, Guid>, BookAppointmentCommandHandler>();
        services.AddScoped<ICommandHandler<RescheduleAppointmentCommand>, RescheduleAppointmentCommandHandler>();
        services.AddScoped<ICommandHandler<ConfirmAppointmentCommand>, ConfirmAppointmentCommandHandler>();
        services.AddScoped<ICommandHandler<CancelAppointmentCommand>, CancelAppointmentCommandHandler>();
        services.AddScoped<ICommandHandler<CompleteAppointmentCommand>, CompleteAppointmentCommandHandler>();
        services.AddScoped<ICommandHandler<MarkAppointmentAsNoShowCommand>, MarkAppointmentAsNoShowCommandHandler>();
        services.AddScoped<IQueryHandler<GetAllAppointmentsQuery, IEnumerable<AppointmentResponse>>, GetAllAppointmentsQueryHandler>();
        services.AddScoped<IQueryHandler<GetAppointmentByIdQuery, AppointmentResponse>, GetAppointmentByIdQueryHandler>();

        return services;
    }
}