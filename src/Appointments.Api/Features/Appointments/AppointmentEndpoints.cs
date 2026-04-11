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

internal class AppointmentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/appointments");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetAppointment");
        group.MapPost("/", Book);
        group.MapPatch("/{id:guid}/cancel", Cancel);
        group.MapPatch("/{id:guid}/complete", Complete);
        group.MapPatch("/{id:guid}/confirm", Confirm);
        group.MapPatch("/{id:guid}/mark-as-no-show", MarkAsNoShow);
        group.MapPatch("/{id:guid}/reschedule", Reschedule);
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
        Guid id,
        [FromServices] ICommandHandler<CancelAppointmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new CancelAppointmentCommand(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Complete(
        Guid id,
        [FromServices] ICommandHandler<CompleteAppointmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new CompleteAppointmentCommand(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Confirm(
        Guid id,
        [FromServices] ICommandHandler<ConfirmAppointmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new ConfirmAppointmentCommand(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> MarkAsNoShow(
        Guid id,
        [FromServices] ICommandHandler<MarkAppointmentAsNoShowCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new MarkAppointmentAsNoShowCommand(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Reschedule(
        Guid id,
        [FromBody] RescheduleAppointmentRequest request,
        [FromServices] ICommandHandler<RescheduleAppointmentCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new RescheduleAppointmentCommand(id, request.NewStartTime);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }
}