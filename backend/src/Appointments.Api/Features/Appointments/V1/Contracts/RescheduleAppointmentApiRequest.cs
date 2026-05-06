namespace Appointments.Api.Features.Appointments.V1.Contracts;

public record RescheduleAppointmentApiRequest(
    DateTimeOffset NewStartTime
);
