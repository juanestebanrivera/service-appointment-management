namespace Appointments.Application.Features.Services;

public record ServiceResult(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration,
    bool IsActive
);