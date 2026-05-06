namespace Appointments.Api.Features.Services.V1.Contracts;

public record UpdateServiceApiRequest(
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration,
    bool IsActive
);