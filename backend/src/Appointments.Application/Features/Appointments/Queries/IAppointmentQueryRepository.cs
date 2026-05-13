using Appointments.Application.Features.Appointments;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries;

public interface IAppointmentQueryRepository : IRepository<Appointment>
{
    Task<IEnumerable<AppointmentDetailResult>> GetByDateAsync(DateTimeOffset date, CancellationToken cancellationToken = default);
    Task<AppointmentDetailResult?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
