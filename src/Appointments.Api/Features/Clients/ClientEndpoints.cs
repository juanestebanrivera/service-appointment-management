using Appointments.Api.Abstractions;
using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Clients;
using Appointments.Application.Features.Clients.Commands.CreateClient;
using Appointments.Application.Features.Clients.Commands.DeleteClient;
using Appointments.Application.Features.Clients.Commands.UpdateClient;
using Appointments.Application.Features.Clients.Queries.GetAllClients;
using Appointments.Application.Features.Clients.Queries.GetClientById;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Features.Clients;

public class ClientEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/clients");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetClient");
        group.MapPost("/", Create);
        group.MapPut("/{id:guid}", Update);
        group.MapDelete("/{id:guid}", Delete);
    }

    private static async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetAllClientsQuery, IEnumerable<ClientResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAllClientsQuery(), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetClientByIdQuery, ClientResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetClientByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> Create(
        [FromBody] CreateClientCommand command,
        [FromServices] ICommandHandler<CreateClientCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.CreatedAtRoute("GetClient", new { id = result.Value });
    }

    private static async Task<IResult> Update(
        Guid id,
        [FromBody] UpdateClientRequest request,
        [FromServices] ICommandHandler<UpdateClientCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateClientCommand(id, request.FirstName, request.LastName, request.Email, request.PhonePrefix, request.PhoneNumber, request.IsActive);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(
        Guid id,
        [FromServices] ICommandHandler<DeleteClientCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new DeleteClientCommand(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }
}