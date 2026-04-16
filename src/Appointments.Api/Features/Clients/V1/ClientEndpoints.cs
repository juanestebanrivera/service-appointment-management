using Appointments.Api.Abstractions;
using Appointments.Api.Extensions;
using Appointments.Api.Features.Clients.V1.Contracts;
using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Clients;
using Appointments.Application.Features.Clients.Commands.CreateClient;
using Appointments.Application.Features.Clients.Commands.DeleteClient;
using Appointments.Application.Features.Clients.Commands.UpdateClient;
using Appointments.Application.Features.Clients.Queries.GetAllClients;
using Appointments.Application.Features.Clients.Queries.GetClientById;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Features.Clients.V1;

internal class ClientEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("clients")
                       .WithTags("Clients");

        group.MapGet("/", GetAll)
             .Produces<IEnumerable<ClientApiResponse>>();

        group.MapGet("/{id:guid}", GetById)
             .WithName("GetClientById")
             .Produces<ClientApiResponse>()
             .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPost("/", Create)
             .Produces<Guid>(StatusCodes.Status201Created)
             .ProducesProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:guid}", Update)
             .Produces(StatusCodes.Status204NoContent)
             .ProducesProblem(StatusCodes.Status400BadRequest)
             .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapDelete("/{id:guid}", Delete)
             .Produces(StatusCodes.Status204NoContent)
             .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetAllClientsQuery, IEnumerable<ClientResult>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAllClientsQuery(), cancellationToken);

        return result.ToApiResult(value => Results.Ok(value.Select(v => v.ToClientApiResponse())));
    }

    private static async Task<IResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetClientByIdQuery, ClientResult> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetClientByIdQuery(id), cancellationToken);

        return result.ToApiResult(value => Results.Ok(value.ToClientApiResponse()));
    }

    private static async Task<IResult> Create(
        [FromBody] CreateClientApiRequest request,
        [FromServices] ICommandHandler<CreateClientCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateClientCommand(request.FirstName, request.LastName, request.PhonePrefix, request.PhoneNumber, request.Email);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(() => Results.CreatedAtRoute("GetClientById", new { id = result.Value }));
    }

    private static async Task<IResult> Update(
        Guid id,
        [FromBody] UpdateClientApiRequest request,
        [FromServices] ICommandHandler<UpdateClientCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateClientCommand(id, request.FirstName, request.LastName, request.Email, request.PhonePrefix, request.PhoneNumber, request.IsActive);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(() => Results.NoContent());
    }

    private static async Task<IResult> Delete(
        Guid id,
        [FromServices] ICommandHandler<DeleteClientCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new DeleteClientCommand(id), cancellationToken);

        return result.ToApiResult(() => Results.NoContent());
    }
}