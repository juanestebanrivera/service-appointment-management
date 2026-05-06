using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments;

public static class AppointmentApplicationErrors
{
    public static readonly Error NotFound = new("Appointment.NotFound", "Appointment was not found.", ErrorType.NotFound);
}
