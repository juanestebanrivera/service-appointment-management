namespace Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;

public record RescheduleAppointmentCommand(
    Guid AppointmentId,
    DateTimeOffset NewStartTime
);
