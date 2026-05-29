using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Services;

public static class DurationMinutesErrors
{
    public static readonly Error DurationMustBeGreaterThanFifteenMinutes = new(ErrorType.Validation, "Duration must be greater than fifteen minutes.");
    public static readonly Error DurationMustBeLessThanEightHours = new(ErrorType.Validation, "Duration must be less than eight hours.");
}