using Appointments.Api.Features.Clients.V1.Contracts;
using Appointments.Api.Infrastructure.Endpoints;
using Appointments.Api.Shared;
using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Appointments.Queries.GetClientAppointmentHistory;
using Appointments.Application.Features.Appointments.Queries.GetClientUpcomingAppointment;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Features.Clients.V1;

internal class ClientAppointmentEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("clients/{id:guid}/appointments")
                       .WithTags("Clients");

        group.MapGet("/", GetHistory)
             .Produces<PagedResponse<ClientAppointmentApiResponse>>()
             .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapGet("/upcoming", GetUpcoming)
             .Produces<ClientUpcomingAppointmentsApiResponse>()
             .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetHistory(
        Guid id,
        [AsParameters] GetClientAppointmentsRequest request,
        [FromServices] IQueryHandler<GetClientAppointmentHistoryQuery, PagedResult<ClientAppointmentResult>> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetClientAppointmentHistoryQuery(id, request.Page, request.Size);
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.ToApiResult(value => Results.Ok(value.ToPagedResponse(a => a.ToClientAppointmentApiResponse())));
    }

    private static async Task<IResult> GetUpcoming(
        Guid id,
        [AsParameters] GetClientUpcomingRequest request,
        [FromServices] IQueryHandler<GetClientUpcomingAppointmentQuery, ClientUpcomingAppointmentsResult> handler,
        CancellationToken cancellationToken)
    {
        var query = new GetClientUpcomingAppointmentQuery(id, request.IncludeLast);
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.ToApiResult(value => Results.Ok(new ClientUpcomingAppointmentsApiResponse
        (
            value.NextAppointment?.ToClientAppointmentApiResponse(),
            value.LastAppointment?.ToClientAppointmentApiResponse()
        )));
    }

}
