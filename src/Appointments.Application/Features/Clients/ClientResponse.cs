namespace Appointments.Application.Features.Clients;

public record ClientResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string PhonePrefix,
    string PhoneNumber,
    string? Email,
    bool IsActive
);