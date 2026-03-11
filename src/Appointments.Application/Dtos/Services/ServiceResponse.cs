namespace Appointments.Application.Dtos.Services;

public record ServiceResponse(
    Guid Id,
    string Name,
    decimal Price,
    TimeSpan EstimatedDuration,
    bool IsActive
);