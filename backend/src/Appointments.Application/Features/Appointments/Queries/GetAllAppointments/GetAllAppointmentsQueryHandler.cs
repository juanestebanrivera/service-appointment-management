using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public sealed class GetAllAppointmentsQueryHandler(IQueryableRepository<Appointment> appointmentRepository)
    : IQueryHandler<GetAllAppointmentsQuery, PagedResult<AppointmentResult>>
{
    private readonly IQueryableRepository<Appointment> _appointmentRepository = appointmentRepository;

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
