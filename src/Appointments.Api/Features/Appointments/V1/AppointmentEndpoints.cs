using Appointments.Api.Abstractions;
using Appointments.Api.Features.Appointments.V1.Contracts;
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

namespace Appointments.Api.Features.Appointments.V1;

internal class AppointmentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("appointments")
                       .WithTags("Appointments");

        group.MapGet("/", GetAll)
             .Produces<IEnumerable<AppointmentApiResponse>>();

        group.MapGet("/{id:guid}", GetById).WithName("GetAppointment")
             .Produces<AppointmentApiResponse>();

        group.MapPost("/", Book)
             .Produces<Guid>(StatusCodes.Status201Created);

        group.MapPatch("/{id:guid}/cancel", Cancel)
             .Produces(StatusCodes.Status204NoContent);

        group.MapPatch("/{id:guid}/complete", Complete)
             .Produces(StatusCodes.Status204NoContent);

        group.MapPatch("/{id:guid}/confirm", Confirm)
             .Produces(StatusCodes.Status204NoContent);

        group.MapPatch("/{id:guid}/mark-as-no-show", MarkAsNoShow)
             .Produces(StatusCodes.Status204NoContent);

        group.MapPatch("/{id:guid}/reschedule", Reschedule)
             .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetAllAppointmentsQuery, IEnumerable<AppointmentResult>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAllAppointmentsQuery(), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        var response = result.Value.Select(a =>
            new AppointmentApiResponse(a.Id, a.ClientId, a.ServiceId, a.PriceAtBooking, a.StartTime, a.EndTime, a.Status));

        return Results.Ok(response);
    }

    private static async Task<IResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetAppointmentByIdQuery, AppointmentResult> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAppointmentByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        var response = new AppointmentApiResponse(
            result.Value.Id,
            result.Value.ClientId,
            result.Value.ServiceId,
            result.Value.PriceAtBooking,
            result.Value.StartTime,
            result.Value.EndTime,
            result.Value.Status);

        return Results.Ok(response);
    }

    private static async Task<IResult> Book(
        [FromBody] BookAppointmentApiRequest request,
        [FromServices] ICommandHandler<BookAppointmentCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new BookAppointmentCommand(request.ClientId, request.ServiceId, request.StartTime);
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
        [FromBody] RescheduleAppointmentApiRequest request,
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