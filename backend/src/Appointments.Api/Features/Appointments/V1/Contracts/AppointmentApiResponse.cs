using Appointments.Domain.Appointments;

namespace Appointments.Api.Features.Appointments.V1.Contracts;

public record AppointmentApiResponse(
    Guid Id,
    Guid ClientId,
    Guid ServiceId,
    decimal PriceAtBooking,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    AppointmentStatus Status
);
