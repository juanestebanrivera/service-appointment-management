namespace Appointments.Application.Features.Clients.Commands.UpdateClient;

public record UpdateClientCommand(
    Guid ClientId,
    string? FirstName,
    string? LastName,
    string? Email,
    string? PhoneNumberPrefix,
    string? PhoneNumber
);
