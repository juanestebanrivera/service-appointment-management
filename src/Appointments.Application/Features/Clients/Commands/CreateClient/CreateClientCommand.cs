namespace Appointments.Application.Features.Clients.Commands.CreateClient;

public record CreateClientCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string PhonePrefix,
    string PhoneNumber,
    string? Email = null
);