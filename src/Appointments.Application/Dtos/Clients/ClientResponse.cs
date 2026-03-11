namespace Appointments.Application.Dtos.Clients;

public record ClientResponse(
    Guid Id,
    string Name,
    string Email,
    string PhoneNumber,
    DateOnly DateOfBirth
);