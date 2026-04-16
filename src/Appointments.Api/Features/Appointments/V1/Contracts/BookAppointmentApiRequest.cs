namespace Appointments.Api.Features.Appointments.V1.Contracts;

public record BookAppointmentApiRequest(
    Guid ClientId,
    Guid ServiceId,
    DateTimeOffset StartTime
);
