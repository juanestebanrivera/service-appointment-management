using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments;

public static class AppointmentApplicationErrors
{
    public static readonly Error NotFound = new(ErrorType.NotFound, "Appointment was not found.");
    public static readonly Error DateIsRequired = new(ErrorType.Validation, "A valid date must be provided.");
    public static readonly Error Forbidden = new(ErrorType.Forbidden, "You don't have permission to access this resource.");
}
