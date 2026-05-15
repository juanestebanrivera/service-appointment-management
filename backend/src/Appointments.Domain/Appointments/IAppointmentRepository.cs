using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<bool> VerifyAvailabilityAsync(DateTimeOffset startTime, DateTimeOffset endTime, Guid? excludeAppointmentId = null, CancellationToken cancellationToken = default);
    Task<bool> HasActiveAppointmentAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByClientAsync(Guid clientId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByServiceAsync(Guid serviceId, CancellationToken cancellationToken = default);

    void Add(Appointment entity);
    void Update(Appointment entity);
}
