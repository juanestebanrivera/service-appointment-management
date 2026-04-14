using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public record TimeRange
{
    public DateTimeOffset StartTime { get; }
    public DateTimeOffset EndTime { get; }
    public TimeSpan Duration => EndTime - StartTime;

    private TimeRange(DateTimeOffset startTime, DateTimeOffset endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
    }

    public static Result<TimeRange> Create(DateTimeOffset startTime, DateTimeOffset endTime, DateTimeOffset currentTime)
    {
        if (startTime < currentTime)
            return Result<TimeRange>.Failure(TimeRangeErrors.CannotBeInThePast);

        if (endTime <= startTime)
            return Result<TimeRange>.Failure(TimeRangeErrors.EndTimeMustBeAfterStartTime);

        if (endTime <= startTime.AddMinutes(5))
            return Result<TimeRange>.Failure(TimeRangeErrors.MustBeMoreThanFiveMinutes);

        if (endTime > startTime.AddDays(1))
            return Result<TimeRange>.Failure(TimeRangeErrors.MustBeLessThanOneDay);

        return Result<TimeRange>.Success(new(startTime, endTime));
    }
}
