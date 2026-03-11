namespace Appointments.Application.Dtos.Services;

public record CreateServiceRequest(
    string Name,
    decimal Price,
    TimeSpan EstimatedDuration
);