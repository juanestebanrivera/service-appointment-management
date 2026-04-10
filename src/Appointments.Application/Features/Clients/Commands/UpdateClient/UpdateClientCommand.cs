namespace Appointments.Application.Features.Clients.Commands.UpdateClient;

public record UpdateClientCommand(
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhonePrefix,
    string? PhoneNumber,
    bool? IsActive
)
{
    public Guid ClientId { get; set; }
};
