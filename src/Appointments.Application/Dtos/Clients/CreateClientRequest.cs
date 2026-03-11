namespace Appointments.Application.Dtos.Clients;

public record CreateClientRequest(
    string Name,
    string Email,
    DateOnly DateOfBith,
    string? PhoneNumber
);