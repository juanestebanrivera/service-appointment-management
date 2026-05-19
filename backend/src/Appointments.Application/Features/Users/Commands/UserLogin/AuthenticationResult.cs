namespace Appointments.Application.Features.Users.Commands.UserLogin;

public record AuthenticationResult(
    Guid UserId,
    string Token
);