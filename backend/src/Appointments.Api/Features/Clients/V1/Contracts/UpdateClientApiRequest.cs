namespace Appointments.Api.Features.Clients.V1.Contracts;

public record UpdateClientApiRequest(
    string FirstName,
    string LastName,
    string? Email,
    string PhonePrefix,
    string PhoneNumber,
    bool IsActive
);
