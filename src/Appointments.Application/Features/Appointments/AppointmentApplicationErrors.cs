using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments;

public static class AppointmentApplicationErrors
{
    public static readonly Error NotFound = new("Appointment.NotFound", "Appointment was not found.");
    public static readonly Error ClientNotFound = new("Appointment.ClientNotFound", "Client was not found.");
    public static readonly Error ServiceNotFound = new("Appointment.ServiceNotFound", "Service was not found.");
}
