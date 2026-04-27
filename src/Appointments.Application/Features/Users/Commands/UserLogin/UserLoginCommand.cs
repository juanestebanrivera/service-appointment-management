namespace Appointments.Application.Features.Users.Commands.UserLogin;

public record UserLoginCommand(
    string Email,
    string Password
);