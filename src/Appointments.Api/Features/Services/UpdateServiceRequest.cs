namespace Appointments.Api.Features.Services;

public record UpdateServiceRequest(
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration,
    bool IsActive
);