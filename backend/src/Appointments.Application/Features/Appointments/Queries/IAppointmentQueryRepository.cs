using Appointments.Application.Common.Pagination;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries;

public interface IAppointmentQueryRepository : IRepository<Appointment>
{
    Task<IEnumerable<AppointmentDetailResult>> GetByDateAsync(DateTimeOffset date, CancellationToken cancellationToken = default);
    Task<AppointmentDetailResult?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<(IEnumerable<ClientAppointmentResult> items, int totalCount)> GetClientAppointmentHistoryAsync(Guid clientId, PaginationParams pagination, CancellationToken cancellationToken = default);
    Task<ClientAppointmentResult?> GetClientUpcomingAppointmentAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<ClientAppointmentResult?> GetClientLastCompletedAppointmentAsync(Guid clientId, CancellationToken cancellationToken = default);
}
