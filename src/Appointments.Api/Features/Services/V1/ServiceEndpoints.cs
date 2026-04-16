using Appointments.Api.Abstractions;
using Appointments.Api.Features.Services.V1.Contracts;
using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Services;
using Appointments.Application.Features.Services.Commands.CreateService;
using Appointments.Application.Features.Services.Commands.DeleteService;
using Appointments.Application.Features.Services.Commands.UpdateService;
using Appointments.Application.Features.Services.Queries.GetAllServices;
using Appointments.Application.Features.Services.Queries.GetServiceById;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Features.Services.V1;

internal class ServiceEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("services")
                       .WithTags("Services");

        group.MapGet("/", GetAll)
             .Produces<IEnumerable<ServiceApiResponse>>();

        group.MapGet("/{id:guid}", GetById)
             .WithName("GetServiceById")
             .Produces<ServiceApiResponse>();

        group.MapPost("/", Create)
             .Produces<Guid>(StatusCodes.Status201Created);

        group.MapPut("/{id:guid}", Update)
             .Produces(StatusCodes.Status204NoContent);

        group.MapDelete("/{id:guid}", Delete)
             .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetAllServicesQuery, IEnumerable<ServiceResult>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAllServicesQuery(), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        var response = result.Value.Select(s =>
            new ServiceApiResponse(
                s.Id,
                s.Name,
                s.Description,
                s.Price,
                s.Duration,
                s.IsActive
            )
        );

        return Results.Ok(response);
    }

    private static async Task<IResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetServiceByIdQuery, ServiceResult> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetServiceByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        var response = new ServiceApiResponse(
            result.Value.Id,
            result.Value.Name,
            result.Value.Description,
            result.Value.Price,
            result.Value.Duration,
            result.Value.IsActive
        );

        return Results.Ok(response);
    }

    private static async Task<IResult> Create(
        [FromBody] CreateServiceApiRequest request,
        [FromServices] ICommandHandler<CreateServiceCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateServiceCommand(request.Name, request.Price, request.Duration, request.Description);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.CreatedAtRoute("GetServiceById", new { id = result.Value });
    }

    private static async Task<IResult> Update(
        Guid id,
        [FromBody] UpdateServiceApiRequest request,
        [FromServices] ICommandHandler<UpdateServiceCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateServiceCommand(id, request.Name, request.Description, request.Price, request.Duration, request.IsActive);
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(
        Guid id,
        [FromServices] ICommandHandler<DeleteServiceCommand> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new DeleteServiceCommand(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

}