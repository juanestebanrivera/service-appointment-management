using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Application.Features.Clients;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetClientAppointmentHistory;

public sealed class GetClientAppointmentHistoryQueryHandler(
    IAppointmentQueryRepository appointmentRepository,
    IClientRepository clientRepository
) : IQueryHandler<GetClientAppointmentHistoryQuery, PagedResult<ClientAppointmentResult>>
{
    private readonly IAppointmentQueryRepository _appointmentRepository = appointmentRepository;
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<Result<PagedResult<ClientAppointmentResult>>> HandleAsync(GetClientAppointmentHistoryQuery query, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(query.ClientId, cancellationToken);

        if (client is null)
            return Result<PagedResult<ClientAppointmentResult>>.Failure(ClientApplicationErrors.NotFound);

        var pagination = new PaginationParams(query.Page, query.PageSize);

        var (items, totalCount) = await _appointmentRepository.GetClientAppointmentHistoryAsync(query.ClientId, pagination, cancellationToken);

        var pagedResult = new PagedResult<ClientAppointmentResult>(items, totalCount, pagination.Page, pagination.PageSize);

        return Result<PagedResult<ClientAppointmentResult>>.Success(pagedResult);
    }
}
