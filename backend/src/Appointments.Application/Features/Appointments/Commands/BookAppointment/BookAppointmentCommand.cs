namespace Appointments.Application.Features.Appointments.Commands.BookAppointment;

public record BookAppointmentCommand(
    Guid ClientId,
    Guid ServiceId,
    DateTimeOffset StartTime
);
