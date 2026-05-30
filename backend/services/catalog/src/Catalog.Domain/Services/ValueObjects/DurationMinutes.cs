using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Services;

public record DurationMinutes
{
    public int Minutes { get; init; }

    private DurationMinutes(int minutes)
    {
        Minutes = minutes;
    }

    public static Result<DurationMinutes> Create(int minutes)
    {
        const int MinimumMinutes = 15;
        const int MaximumMinutes = 480; // 8 hours

        if (minutes <= MinimumMinutes)
            return Result<DurationMinutes>.Failure(DurationMinutesErrors.DurationMustBeGreaterThanFifteenMinutes);

        if (minutes > MaximumMinutes)
            return Result<DurationMinutes>.Failure(DurationMinutesErrors.DurationMustBeLessThanEightHours);

        return Result<DurationMinutes>.Success(new DurationMinutes(minutes));
    }
}