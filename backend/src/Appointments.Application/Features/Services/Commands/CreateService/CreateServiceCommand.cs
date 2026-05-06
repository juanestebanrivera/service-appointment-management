namespace Appointments.Application.Features.Services.Commands.CreateService;

public record CreateServiceCommand(
    string Name,
    decimal Price,
    TimeSpan Duration,
    string? Description = null
);
