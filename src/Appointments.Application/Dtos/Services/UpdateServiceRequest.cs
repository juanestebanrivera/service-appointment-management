namespace Appointments.Application.Dtos.Services;

public record UpdateServiceRequest(
    string Name,
    decimal Price,
    TimeSpan EstimatedDuration,
    bool IsActive
);