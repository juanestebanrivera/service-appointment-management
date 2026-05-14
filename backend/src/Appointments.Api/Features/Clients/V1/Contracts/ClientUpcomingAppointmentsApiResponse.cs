namespace Appointments.Api.Features.Clients.V1.Contracts;

public record ClientUpcomingAppointmentsApiResponse(
    ClientAppointmentApiResponse? NextAppointment,
    ClientAppointmentApiResponse? LastAppointment
);