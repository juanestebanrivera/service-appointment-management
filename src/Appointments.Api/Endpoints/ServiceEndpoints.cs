using Appointments.Api.Abstractions;
using Appointments.Application.Features.Services.Commands.CreateService;
using Appointments.Application.Features.Services.Commands.DeleteService;
using Appointments.Application.Features.Services.Commands.UpdateService;
using Appointments.Application.Features.Services.Queries.GetAllServices;
using Appointments.Application.Features.Services.Queries.GetServiceById;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Endpoints;

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
        IGetAllServicesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var services = await handler.HandleAsync(cancellationToken);
        
        return Results.Ok(services);
    }

    private static async Task<IResult> GetById(
        Guid id,
        IGetServiceByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(id, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.Ok(result.Value);
    }

    private static async Task<IResult> Create(
        CreateServiceCommand command,
        [FromServices] ICreateServiceCommandHandler handler, 
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.CreatedAtRoute("GetService", new { id = result.Value });
    }

    private static async Task<IResult> Update(
        Guid id,
        [FromBody] UpdateServiceCommand command,
        [FromServices] IUpdateServiceCommandHandler handler,
        CancellationToken cancellationToken)
    {
        command.ServiceId = id;
        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(
        Guid id,
        [FromServices] IDeleteServiceCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new DeleteServiceCommand(id), cancellationToken);

        if (result.IsFailure)
            return Results.BadRequest(result.Error);

        return Results.NoContent();
    }
    
}