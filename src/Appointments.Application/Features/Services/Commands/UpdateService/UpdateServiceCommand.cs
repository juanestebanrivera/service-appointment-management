namespace Appointments.Application.Features.Services.Commands.UpdateService;

public record UpdateServiceCommand(
    Guid ServiceId,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration,
    bool IsActive
);
