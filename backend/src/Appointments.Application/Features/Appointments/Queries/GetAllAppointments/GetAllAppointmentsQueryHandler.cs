using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public sealed class GetAllAppointmentsQueryHandler(IAppointmentQueryRepository appointmentRepository)
    : IQueryHandler<GetAllAppointmentsQuery, PagedResult<AppointmentResult>>
{
    private readonly IAppointmentQueryRepository _appointmentRepository = appointmentRepository;

    public async Task<Result<PagedResult<AppointmentResult>>> HandleAsync(GetAllAppointmentsQuery query, CancellationToken cancellationToken = default)
    {
        var pagination = new PaginationParams(query.Page, query.PageSize);

        var (items, totalCount) = await _appointmentRepository.GetPagedAsync(pagination, query.SearchTerm, cancellationToken);

        var pagedResult = new PagedResult<AppointmentResult>
        (
            items.Select(a => a.ToAppointmentResult()),
            totalCount,
            pagination.Page,
            pagination.PageSize
        );

        return Result<PagedResult<AppointmentResult>>.Success(pagedResult);
    }
}
