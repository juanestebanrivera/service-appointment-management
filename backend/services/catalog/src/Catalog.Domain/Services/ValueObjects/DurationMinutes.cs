using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Services;

public record DurationMinutes
{
    public int Value { get; init; }

    private DurationMinutes(int value)
    {
        Value = value;
    }

    public static Result<DurationMinutes> Create(int value)
    {
        const int MinimumDuration = 15;
        const int MaximumDuration = 480; // 8 hours

        if (value <= MinimumDuration)
            return Result<DurationMinutes>.Failure(DurationMinutesErrors.DurationMustBeGreaterThanFifteenMinutes);

        if (value > MaximumDuration)
            return Result<DurationMinutes>.Failure(DurationMinutesErrors.DurationMustBeLessThanEightHours);

        return Result<DurationMinutes>.Success(new DurationMinutes(value));
    }
}