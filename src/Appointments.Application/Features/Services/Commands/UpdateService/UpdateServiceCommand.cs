namespace Appointments.Application.Features.Services.Commands.UpdateService;

public record UpdateServiceCommand(
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration,
    bool IsActive
)
{
    public Guid ServiceId { get; set; }
}
