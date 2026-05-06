namespace Appointments.Api.Features.Users.V1.Contracts;

public record UserRegisterApiRequest(
    string Email,
    string Password
);