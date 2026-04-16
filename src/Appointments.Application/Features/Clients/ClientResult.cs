namespace Appointments.Application.Features.Clients;

public record ClientResult(
    Guid Id,
    string FirstName,
    string LastName,
    string PhonePrefix,
    string PhoneNumber,
    string? Email,
    bool IsActive
);