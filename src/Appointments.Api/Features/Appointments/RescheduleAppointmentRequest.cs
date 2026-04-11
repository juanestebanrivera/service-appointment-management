namespace Appointments.Api.Features.Appointments;

public record RescheduleAppointmentRequest(
    DateTimeOffset NewStartTime
);