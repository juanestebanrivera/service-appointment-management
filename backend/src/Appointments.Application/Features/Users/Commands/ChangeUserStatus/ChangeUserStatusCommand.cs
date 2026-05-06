namespace Appointments.Application.Features.Users.Commands.ChangeUserStatus;

public record ChangeUserStatusCommand(
    Guid UserId,
    bool IsActive
);