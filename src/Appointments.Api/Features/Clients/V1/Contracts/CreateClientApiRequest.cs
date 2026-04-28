namespace Appointments.Api.Features.Clients.V1.Contracts;

public record CreateClientApiRequest(
    Guid UserId,
    string FirstName,
    string LastName,
    string PhonePrefix,
    string PhoneNumber,
    string? Email = null
);
