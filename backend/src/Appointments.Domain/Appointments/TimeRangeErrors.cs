using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public static class TimeRangeErrors
{
    public static readonly Error CannotBeInThePast = new(ErrorType.Validation, "Time cannot be in the past.");
    public static readonly Error MustBeMoreThanFiveMinutes = new(ErrorType.Validation, "Time must be more than five minutes.");
    public static readonly Error MustBeLessThanOneDay = new(ErrorType.Validation, "Time must be less than one day.");
    public static readonly Error EndTimeMustBeAfterStartTime = new(ErrorType.Validation, "End time must be after start time.");
}
