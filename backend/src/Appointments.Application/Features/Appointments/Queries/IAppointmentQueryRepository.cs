using Appointments.Application.Common.Pagination;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries;

public interface IAppointmentQueryRepository : IRepository<Appointment>
{
    Task<(IEnumerable<Appointment> Items, int TotalCount)> GetPagedAsync(PaginationParams pagination, string? searchQuery = null, CancellationToken cancellationToken = default);
}
