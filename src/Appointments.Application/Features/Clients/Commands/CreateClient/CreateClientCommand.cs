namespace Appointments.Application.Features.Clients.Commands.CreateClient;

public record CreateClientCommand(
    string FirstName,
    string LastName,
    string PhonePrefix,
    string PhoneNumber,
    string? Email = null
);