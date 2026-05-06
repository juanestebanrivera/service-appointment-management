namespace Appointments.Api.Features.Users.V1.Contracts;

public record UserLoginApiRequest(
    string Email,
    string Password
);