namespace Appointments.Application.Features.Services;

public record ServiceResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration,
    bool IsActive
);