namespace Appointments.Api.Features.Services.V1.Contracts;

public record CreateServiceApiRequest(
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration
);