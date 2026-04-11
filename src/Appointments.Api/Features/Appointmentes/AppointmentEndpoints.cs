using Appointments.Api.Abstractions;
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
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Features.Appointments;

public class AppointmentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
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
        [FromServices] IQueryHandler<GetAllAppointmentsQuery, IEnumerable<AppointmentResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAllAppointmentsQuery(), cancellationToken);
        
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetAppointmentByIdQuery, AppointmentResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAppointmentByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> Book(
        [FromBody] BookAppointmentCommand command,
        [FromServices] ICommandHandler<BookAppointmentCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.CreatedAtRoute("GetAppointment", new { id = result.Value });
    }

    private static async Task<IResult> Cancel(
        [AsParameters] CancelAppointmentCommand command,
        [FromServices] ICommandHandler<CancelAppointmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Complete(
        [AsParameters] CompleteAppointmentCommand command,
        [FromServices] ICommandHandler<CompleteAppointmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Confirm(
        [AsParameters] ConfirmAppointmentCommand command,
        [FromServices] ICommandHandler<ConfirmAppointmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> MarkAsNoShow(
        [AsParameters] MarkAppointmentAsNoShowCommand command,
        [FromServices] ICommandHandler<MarkAppointmentAsNoShowCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Reschedule(
        [FromBody] RescheduleAppointmentCommand command,
        [FromServices] ICommandHandler<RescheduleAppointmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }
}