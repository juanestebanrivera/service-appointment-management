using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Users;

public static class UserApplicationErrors
{
    public static readonly Error InvalidCredentials = new("InvalidCredentials", "The email or password is incorrect.", ErrorType.Unauthorized);
    public static readonly Error InvalidEmail = new("InvalidEmail", "The provided email is not valid. Please provide another email.", ErrorType.Validation);

    public static readonly Error UserNotFound = new("UserNotFound", "The user was not found.", ErrorType.NotFound);
}