using Appointments.Application.Features.Appointments.Commands.BookAppointment;
using Appointments.Application.Features.Appointments.Commands.CancelAppointment;
using Appointments.Application.Features.Appointments.Commands.CompleteAppointment;
using Appointments.Application.Features.Appointments.Commands.ConfirmAppointment;
using Appointments.Application.Features.Appointments.Commands.MarkAppointmentAsNoShow;
using Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentById;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Endpoints;

public static class AppointmentEndpoints
{
    public static void MapAppointmentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/appointments");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetAppointment");
        group.MapPost("/", Book);
        group.MapPatch("/cancel", Cancel);
        group.MapPatch("/complete", Complete);
        group.MapPatch("/confirm", Confirm);
        group.MapPatch("/mark-as-no-show", MarkAsNoShow);
        group.MapPatch("/reschedule", Reschedule);
    }

    private static async Task<IResult> GetAll(
        IGetAllAppointmentsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var appointments = await handler.HandleAsync(cancellationToken);
        
        return Results.Ok(appointments);
    }

    private static async Task<IResult> GetById(
        Guid id,
        IGetAppointmentByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> Book(
        [FromBody] BookAppointmentCommand command,
        [FromServices] IBookAppointmentCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.CreatedAtRoute("GetAppointment", new { id = result.Value });
    }

    private static async Task<IResult> Cancel(
        [AsParameters] CancelAppointmentCommand command,
        [FromServices] ICancelAppointmentCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Complete(
        [AsParameters] CompleteAppointmentCommand command,
        [FromServices] ICompleteAppointmentCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Confirm(
        [AsParameters] ConfirmAppointmentCommand command,
        [FromServices] IConfirmAppointmentCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> MarkAsNoShow(
        [AsParameters] MarkAppointmentAsNoShowCommand command,
        [FromServices] IMarkAppointmentAsNoShowCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Reschedule(
        [FromBody] RescheduleAppointmentCommand command,
        [FromServices] IRescheduleAppointmentCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }
}