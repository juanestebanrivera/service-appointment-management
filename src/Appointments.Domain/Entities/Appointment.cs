using Appointments.Domain.Common;
using Appointments.Domain.Enums;
using Appointments.Domain.Exceptions;

namespace Appointments.Domain.Entities;

public class Appointment : BaseEntity
{
    public Guid ClientId { get; private set; }
    public Client? Client { get; private set; }
    public Guid ServiceId { get; private set; }
    public Service? Service { get; private set; }
    public DateTimeOffset ScheduledTime { get; private set; }
    public TimeSpan Duration { get; private set; }
    public AppointmentStatus Status { get; private set; } = AppointmentStatus.Pending;

    public Appointment(Guid clientId, Guid serviceId)
    {
        if (clientId == Guid.Empty)
            throw new DomainValidationException("Client id is required", nameof(ClientId));

        if (serviceId == Guid.Empty)
            throw new DomainValidationException("Service id is required", nameof(ServiceId));

        ClientId = clientId;
        ServiceId = serviceId;
    }

    public void ScheduleService(DateTimeOffset scheduledTime, TimeSpan duration)
    {
        if (scheduledTime < DateTimeOffset.Now)
            throw new DomainValidationException("Scheduled time cannot book a past date", nameof(ScheduledTime));

        if (scheduledTime > DateTimeOffset.Now.AddYears(1))
            throw new DomainValidationException("Scheduled time cannot book for a year later", nameof(ScheduledTime));

        if (duration.Hours > TimeSpan.HoursPerDay)
            throw new DomainValidationException("Duration cannot be longer than one day", nameof(Duration));

        ScheduledTime = scheduledTime;
        Duration = duration;
    }

    public void UpdateStatus(AppointmentStatus status)
    {
        Status = status;
    }
}
