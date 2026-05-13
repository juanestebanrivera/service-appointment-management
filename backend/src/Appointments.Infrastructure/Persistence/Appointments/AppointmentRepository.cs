using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Appointments.Queries;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Appointments;

internal sealed class AppointmentRepository(ApplicationDbContext dbContext) : IAppointmentRepository, IAppointmentQueryRepository
{
    private readonly DbSet<Appointment> _appointments = dbContext.Set<Appointment>();
    private readonly DbSet<Client> _clients = dbContext.Set<Client>();
    private readonly DbSet<Service> _services = dbContext.Set<Service>();

    public async Task<IEnumerable<AppointmentDetailResult>> GetByDateAsync(DateTimeOffset date, CancellationToken cancellationToken = default)
    {
        return await _appointments
            .AsNoTracking()
            .Where(a => a.TimeRange.StartTime.Date == date.Date)
            .Join(_clients.AsNoTracking(), a => a.ClientId, c => c.Id, (appointment, client) => new { appointment, client })
            .Join(_services.AsNoTracking(), r => r.appointment.ServiceId, s => s.Id, (result, service) => new { result.appointment, result.client, service })
            .Select(r => new AppointmentDetailResult(
                r.appointment.Id,
                r.appointment.PriceAtBooking,
                r.appointment.TimeRange.StartTime,
                r.appointment.TimeRange.EndTime,
                r.appointment.Status,
                r.client.Id,
                r.client.FirstName.Value,
                r.client.LastName.Value,
                r.client.Email != null ? r.client.Email.Value : null,
                r.client.Phone.Prefix,
                r.client.Phone.Number,
                r.service.Id,
                r.service.Name
            )).ToListAsync(cancellationToken);
    }

    public async Task<AppointmentDetailResult?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _appointments
            .Where(a => a.Id == id)
            .Join(_clients.AsNoTracking(), a => a.ClientId, c => c.Id, (appointment, client) => new { appointment, client })
            .Join(_services.AsNoTracking(), r => r.appointment.ServiceId, s => s.Id, (result, service) => new { result.appointment, result.client, service })
            .Select(r => new AppointmentDetailResult(
                r.appointment.Id,
                r.appointment.PriceAtBooking,
                r.appointment.TimeRange.StartTime,
                r.appointment.TimeRange.EndTime,
                r.appointment.Status,
                r.client.Id,
                r.client.FirstName.Value,
                r.client.LastName.Value,
                r.client.Email != null ? r.client.Email.Value : null,
                r.client.Phone.Prefix,
                r.client.Phone.Number,
                r.service.Id,
                r.service.Name
            )).FirstOrDefaultAsync(cancellationToken);
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
}
