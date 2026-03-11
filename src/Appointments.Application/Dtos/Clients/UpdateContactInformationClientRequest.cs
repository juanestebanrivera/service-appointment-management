namespace Appointments.Application.Dtos.Clients;

public record UpdateContactInformationClientRequest(
    string? Email,
    string? PhoneNumber
);