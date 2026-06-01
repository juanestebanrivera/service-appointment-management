using Catalog.Domain.Establishments;

namespace Catalog.Api.Features.Establishments.V1;

public record WeeklyScheduleResponse(
    DayOfWeek Day,
    TimeSpan OpeningTime,
    TimeSpan ClosingTime
)
{
    public static WeeklyScheduleResponse From(WeeklySchedule schedule)
    {
        return new WeeklyScheduleResponse
        (
            schedule.DayOfWeek,
            schedule.OpeningTime,
            schedule.ClosingTime
        );
    }
}