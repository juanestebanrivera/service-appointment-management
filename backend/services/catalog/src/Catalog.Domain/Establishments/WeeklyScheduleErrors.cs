using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Establishments;

public static class WeeklyScheduleErrors
{
    public static readonly Error OpeningTimeMustBeBeforeClosingTime = new(ErrorType.Validation, "Opening time must be before closing time.");
}