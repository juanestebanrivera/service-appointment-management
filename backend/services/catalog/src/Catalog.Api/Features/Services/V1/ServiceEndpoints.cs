using Catalog.Api.Features.Services.V1.Requests;
using Catalog.Api.Features.Services.V1.Responses;
using Catalog.Api.Infrastructure.Endpoints;
using Catalog.Api.Shared;
using Catalog.Application.Abstractions;
using Catalog.Application.Services.Commands;
using Catalog.Application.Services.Queries;
using Catalog.Domain.SharedKernel;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Services.V1;

public class ServiceEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/services")
                           .WithTags("Services");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetServiceById");
        group.MapPost("/", Create);
        group.MapPut("/{id:guid}", Update);
        group.MapDelete("/{id:guid}", Delete);
    }

    private async Task<IResult> GetAll(
        [AsParameters] GetServicesRequest request,
        [FromServices] IQueryHandler<GetAllServicesQuery, PagedList<ServiceResult>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetAllServicesQuery(request.Page, request.Size, request.Search);
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.ToApiResult(v =>
            Results.Ok(PagedResponse<ServiceResponse>.From(v, ServiceResponse.From)));
    }

    private async Task<IResult> GetById(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<GetServiceByIdQuery, ServiceResult> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetServiceByIdQuery(id);
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.ToApiResult(v => Results.Ok(ServiceResponse.From(v)));
    }

    private async Task<IResult> Create(
        [FromBody] CreateServiceApiRequest request,
        [FromServices] ICommandHandler<CreateServiceCommand, Guid> handler,
        CancellationToken cancellationToken)
    {
        var command = new CreateServiceCommand(
            request.Name,
            request.Description,
            request.Price,
            request.Currency,
            request.Minutes);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(id =>
            Results.CreatedAtRoute("GetServiceById", new { id }, null));
    }

    private async Task<IResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateServiceApiRequest request,
        [FromServices] ICommandHandler<UpdateServiceCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateServiceCommand(
            id,
            request.Name,
            request.Description,
            request.Price,
            request.Currency,
            request.Minutes);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(Results.NoContent);
    }

    private async Task<IResult> Delete(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<DeactivateServiceCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new DeactivateServiceCommand(id);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(Results.NoContent);
    }
}