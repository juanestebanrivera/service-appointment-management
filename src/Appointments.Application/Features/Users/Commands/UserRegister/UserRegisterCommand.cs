namespace Appointments.Application.Features.Users.Commands.UserRegister;

public record UserRegisterCommand(
    string Email,
    string Password
);