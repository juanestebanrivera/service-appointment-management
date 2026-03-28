namespace Appointments.Application.Features.Clients.Commands.CreateClient;

public record CreateClientCommand(
    string FirstName,
    string LastName,
    string PhoneNumberPrefix,
    string PhoneNumber,
    string? Email = null
);