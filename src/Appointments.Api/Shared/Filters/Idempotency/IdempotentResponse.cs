namespace Appointments.Api.Shared.Filters.Idempotency;

public record IdempotentResponse(
    IdempotencyStatus Status,
    int? StatusCode = null,
    object? Body = null,
    Dictionary<string, string>? Headers = null
);

public enum IdempotencyStatus
{
    InProgress,
    Completed
}