using Appointments.Domain.Appointments;

namespace Appointments.Application.Features.Appointments;

public record AppointmentDetailResult(
    Guid Id,
    decimal PriceAtBooking,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    AppointmentStatus Status,
    Guid ClientId,
    Guid ClientUserId,
    string ClientFirstName,
    string ClientLastName,
    string? ClientEmail,
    string ClientPhonePrefix,
    string ClientPhoneNumber,
    Guid ServiceId,
    string ServiceName
);
