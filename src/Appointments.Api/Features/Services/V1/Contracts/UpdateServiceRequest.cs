namespace Appointments.Api.Features.Services.V1;

public record UpdateServiceRequest(
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration,
    bool IsActive
);