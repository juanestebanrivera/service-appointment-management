using Appointments.Api.Abstractions;
using Appointments.Application.Features.Clients.Commands.CreateClient;
using Appointments.Application.Features.Clients.Commands.DeleteClient;
using Appointments.Application.Features.Clients.Commands.UpdateClient;
using Appointments.Application.Features.Clients.Queries.GetAllClients;
using Appointments.Application.Features.Clients.Queries.GetClientById;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Endpoints;

public class ClientEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/clients");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetClient");
        group.MapPost("/", Create);
        group.MapPatch("/{id:guid}", Update);
        group.MapDelete("/{id:guid}", Delete);
    }

    private static async Task<IResult> GetAll(
        IGetAllClientsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var clients = await handler.HandleAsync(new GetAllClientsQuery(), cancellationToken);
        
        return Results.Ok(clients);
    }

    private static async Task<IResult> GetById(
        Guid id,
        IGetClientByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetClientByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> Create(
        [FromBody] CreateClientCommand command,
        [FromServices] ICreateClientCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.CreatedAtRoute("GetClient", new { id = result.Value });
    }

    private static async Task<IResult> Update(
        Guid id,
        [FromBody] UpdateClientCommand command,
        [FromServices] IUpdateClientCommandHandler handler,
        CancellationToken cancellationToken)
    {
        command.ClientId = id;
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(
        Guid id,
        IDeleteClientCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new DeleteClientCommand(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }
}