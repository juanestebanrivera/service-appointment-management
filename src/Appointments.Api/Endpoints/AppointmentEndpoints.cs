using Appointments.Application.Dtos.Appointments;
using Appointments.Application.Interfaces.Services;

namespace Appointments.Api.Endpoints;

public static class AppointmentEndpoints
{
    public static void MapAppointmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/appointments");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetAppointment");
        group.MapPost("/", Create);
        group.MapPatch("/{id:guid}/reschedule", Reschedule);
        group.MapPatch("/{id:guid}/status", UpdateStatus);
        group.MapDelete("/{id:guid}", Delete);
    }

    private static async Task<IResult> GetAll(IAppointmentService service)
    {
        var appointmentsResponse = await service.GetAllAsync();
        
        return Results.Ok(appointmentsResponse);
    }

    private static async Task<IResult> GetById(Guid id, IAppointmentService service)
    {
        var appointmentResponse = await service.GetByIdAsync(id);

        if (appointmentResponse is null)
            return Results.NotFound();

        return Results.Ok(appointmentResponse);
    }

    private static async Task<IResult> Create(CreateAppointmentRequest request, IAppointmentService service)
    {
        var appointmentResponse = await service.CreateAsync(request);
        return Results.CreatedAtRoute("GetAppointment", new { id = appointmentResponse.Id }, appointmentResponse);
    }

    private static async Task<IResult> Reschedule(Guid id, RescheduleAppointmentRequest request, IAppointmentService service)
    {
        var appointmentResponse = await service.Reschedule(id, request);

        if (appointmentResponse is null)
            return Results.NotFound();

        return Results.NoContent();
    }

    private static async Task<IResult> UpdateStatus(Guid id, UpdateStatusAppointmentRequest request, IAppointmentService service)
    {
        var appointmentResponse = await service.UpdateStatusAsync(id, request);

        if (appointmentResponse is null)
            return Results.NotFound();

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, IAppointmentService service)
    {
        var appointmentResponse = await service.DeleteAsync(id);

        if (appointmentResponse is null)
            return Results.NotFound();

        return Results.NoContent();
    }
}