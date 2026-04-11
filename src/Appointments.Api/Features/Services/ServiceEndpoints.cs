using Appointments.Api.Abstractions;
using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Services;
using Appointments.Application.Features.Services.Commands.CreateService;
using Appointments.Application.Features.Services.Commands.DeleteService;
using Appointments.Application.Features.Services.Commands.UpdateService;
using Appointments.Application.Features.Services.Queries.GetAllServices;
using Appointments.Application.Features.Services.Queries.GetServiceById;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Features.Services;

public class ServiceEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/services");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetService");
        group.MapPost("/", Create);
        group.MapPut("/{id:guid}", Update);
        group.MapDelete("/{id:guid}", Delete);
    }

    private static async Task<IResult> GetAll(
        [FromServices] IQueryHandler<GetAllServicesQuery, IEnumerable<ServiceResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetAllServicesQuery(), cancellationToken);
        
        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetServiceByIdQuery, ServiceResponse> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetServiceByIdQuery(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> Create(
        CreateServiceCommand command,
        [FromServices] ICommandHandler<CreateServiceCommand, Guid> handler, 
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.CreatedAtRoute("GetService", new { id = result.Value });
    }

    private static async Task<IResult> Update(
        Guid id,
        [FromBody] UpdateServiceRequest request,
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