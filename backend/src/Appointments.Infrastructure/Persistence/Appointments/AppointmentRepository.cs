using Appointments.Domain.Appointments;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Appointments;

internal sealed class AppointmentRepository(ApplicationDbContext dbContext) : IAppointmentRepository
{
    private readonly DbSet<Appointment> _appointments = dbContext.Set<Appointment>();

    public async Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _appointments.ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _appointments.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<bool> VerifyAvailabilityAsync(DateTimeOffset startTime, DateTimeOffset endTime, Guid? excludeAppointmentId = null, CancellationToken cancellationToken = default)
    {
        return !await _appointments.AnyAsync(a =>
            a.Id != excludeAppointmentId &&
            ((startTime >= a.TimeRange.StartTime && startTime < a.TimeRange.EndTime) ||
             (endTime > a.TimeRange.StartTime && endTime <= a.TimeRange.EndTime) ||
             (startTime <= a.TimeRange.StartTime && endTime >= a.TimeRange.EndTime)),
            cancellationToken);
    }

    public void Add(Appointment appointment)
    {
        _appointments.Add(appointment);
    }

    public void Update(Appointment appointment)
    {
        _appointments.Update(appointment);
    }

    public void Delete(Appointment appointment)
    {
        _appointments.Remove(appointment);
    }
}