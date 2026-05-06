using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public interface IAppointmentRepository : IRepository<Appointment>
{
    Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> VerifyAvailabilityAsync(DateTimeOffset startTime, DateTimeOffset endTime, Guid? excludeAppointmentId = null, CancellationToken cancellationToken = default);

    void Add(Appointment entity);
    void Update(Appointment entity);
    void Delete(Appointment entity);
}
