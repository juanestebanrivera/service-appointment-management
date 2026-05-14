using Appointments.Domain.Appointments;

namespace Appointments.Application.Features.Appointments;

public record ClientAppointmentResult(
    Guid Id,
    decimal PriceAtBooking,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    AppointmentStatus Status,
    Guid ServiceId,
    string ServiceName
);
