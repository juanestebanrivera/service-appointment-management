using Catalog.Api.Infrastructure.Endpoints;
using Catalog.Api.Shared;
using Catalog.Application.Abstractions;
using Catalog.Application.Establishments.Commands;
using Catalog.Application.Establishments.Queries;
using Catalog.Domain.Establishments;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Features.Establishments.V1;

public class EstablishmentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("/establishments")
                           .WithTags("Establishments");

        group.MapGet("/", Get);
        group.MapGet("/weekly-schedule", GetWeeklySchedule);
        group.MapPut("/{id:guid}", Update);
        group.MapPut("/{id:guid}/weekly-schedule", UpdateWeeklySchedule);
    }

    private async Task<IResult> Get(
        [FromServices] IQueryHandler<GetEstablishmentQuery, Establishment> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetEstablishmentQuery(), cancellationToken);

        return result.ToApiResult(v => Results.Ok(EstablishmentResponse.From(v)));
    }

    private async Task<IResult> GetWeeklySchedule(
        [FromServices] IQueryHandler<GetWeeklyScheduleQuery, IEnumerable<WeeklySchedule>> handler,
        CancellationToken cancellationToken
    )
    {
        var result = await handler.HandleAsync(new GetWeeklyScheduleQuery(), cancellationToken);

        return result.ToApiResult(v => Results.Ok(v.Select(WeeklyScheduleResponse.From)));
    }

    private async Task<IResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateEstablishmentRequest request,
        [FromServices] ICommandHandler<UpdateEstablishmentCommand> handler,
        CancellationToken cancellationToken
    )
    {
        var command = new UpdateEstablishmentCommand(id, request.Name, request.Address, request.Phone);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(() => Results.NoContent());
    }

    private async Task<IResult> UpdateWeeklySchedule(
        [FromRoute] Guid id,
        [FromBody] IEnumerable<UpdateWeeklyScheduleRequest> request,
        [FromServices] ICommandHandler<UpdateWeeklyScheduleCommand> handler,
        CancellationToken cancellationToken
    )
    {
        var weeklySchedules = request.Select(r => new WeeklyScheduleDto(r.Day, r.OpeningTime, r.ClosingTime));
        var command = new UpdateWeeklyScheduleCommand(id, weeklySchedules);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(() => Results.NoContent());
    }
}