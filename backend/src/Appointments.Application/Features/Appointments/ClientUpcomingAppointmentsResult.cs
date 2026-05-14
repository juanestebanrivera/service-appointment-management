namespace Appointments.Application.Features.Appointments;

public record ClientUpcomingAppointmentsResult(
    ClientAppointmentResult? NextAppointment,
    ClientAppointmentResult? LastAppointment
);
