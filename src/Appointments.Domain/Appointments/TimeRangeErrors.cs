using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public static class TimeRangeErrors
{
    public static readonly Error CannotBeInThePast = new("TimeRange.CannotBeInThePast", "Time cannot be in the past.");
    public static readonly Error MustBeMoreThanFiveMinutes = new("TimeRange.MustBeMoreThanFiveMinutes", "Time must be more than five minutes.");
    public static readonly Error MustBeLessThanOneDay = new("TimeRange.MustBeLessThanOneDay", "Time must be less than one day.");
    public static readonly Error EndTimeMustBeAfterStartTime = new("TimeRange.EndTimeMustBeAfterStartTime", "End time must be after start time.");
}
