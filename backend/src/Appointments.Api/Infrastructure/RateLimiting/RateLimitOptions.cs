namespace Appointments.Api.Infrastructure.RateLimiting;

public sealed class RateLimitOptions
{
    public const string SectionName = "RateLimiting";

    public GlobalRateLimitOptions Global { get; set; } = new();
}

public sealed class GlobalRateLimitOptions
{
    public int PermitLimit { get; set; } = 60;
    public int WindowInSeconds { get; set; } = 60;
    public int SegmentsPerWindow { get; set; } = 6;
    public int QueueLimit { get; set; } = 0;
}