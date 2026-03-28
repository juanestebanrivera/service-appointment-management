using Appointments.Domain.Appointments;

namespace Appointments.Application.Features.Appointments;

public record AppointmentResponse(
    Guid Id,
    Guid ClientId,
    Guid ServiceId,
    decimal PriceAtBooking,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    AppointmentStatus Status
);