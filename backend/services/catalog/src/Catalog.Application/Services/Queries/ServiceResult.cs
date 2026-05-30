namespace Catalog.Application.Services.Queries;

public record ServiceResult(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    int DurationMinutes
);