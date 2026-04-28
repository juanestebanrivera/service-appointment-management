namespace Appointments.Infrastructure.Authentication;

public sealed class JwtConfiguration
{
    public const string SectionName = "JwtSettings";

    public string Secret { get; init; } = string.Empty;
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
    public int ExpirationMinutes { get; init; }
}