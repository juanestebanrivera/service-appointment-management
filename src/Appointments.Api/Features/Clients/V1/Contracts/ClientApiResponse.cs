namespace Appointments.Api.Features.Clients.V1.Contracts;

public record ClientApiResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string PhonePrefix,
    string PhoneNumber,
    string? Email,
    bool IsActive
);
