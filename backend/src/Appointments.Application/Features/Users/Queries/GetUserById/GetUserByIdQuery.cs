namespace Appointments.Application.Features.Users.Queries.GetUserById;

public record GetUserByIdQuery(
    Guid UserId,
    Guid CurrentUserId,
    bool IsAdmin
);
