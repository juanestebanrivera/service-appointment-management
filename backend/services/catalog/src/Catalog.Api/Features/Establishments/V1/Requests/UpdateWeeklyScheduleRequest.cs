namespace Catalog.Api.Features.Establishments.V1;

public record UpdateWeeklyScheduleRequest(
    DayOfWeek Day,
    TimeSpan OpeningTime,
    TimeSpan ClosingTime
);