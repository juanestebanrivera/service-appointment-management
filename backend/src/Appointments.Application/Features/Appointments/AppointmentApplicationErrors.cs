using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments;

public static class AppointmentApplicationErrors
{
    public static readonly Error NotFound = new("Appointment.NotFound", "Appointment was not found.", ErrorType.NotFound);
    public static readonly Error DateIsRequired = new("Appointment.DateIsRequired", "A valid date must be provided.", ErrorType.Validation);
}
