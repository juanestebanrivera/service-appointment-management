using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Establishments;

public record WeeklySchedule
{
    public DayOfWeek DayOfWeek { get; init; }
    public TimeSpan OpeningTime { get; init; }
    public TimeSpan ClosingTime { get; init; }

    private WeeklySchedule(DayOfWeek dayOfWeek, TimeSpan openingTime, TimeSpan closingTime)
    {
        DayOfWeek = dayOfWeek;
        OpeningTime = openingTime;
        ClosingTime = closingTime;
    }

    public static Result<WeeklySchedule> Create(DayOfWeek dayOfWeek, TimeSpan openingTime, TimeSpan closingTime)
    {
        if (openingTime >= closingTime)
            return Result<WeeklySchedule>.Failure(WeeklyScheduleErrors.OpeningTimeMustBeBeforeClosingTime);

        var schedule = new WeeklySchedule(dayOfWeek, openingTime, closingTime);
        return Result<WeeklySchedule>.Success(schedule);
    }
}