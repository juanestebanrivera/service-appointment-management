using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Users;

public static class UserApplicationErrors
{
    public static readonly Error InvalidCredentials = new(ErrorType.Unauthorized, "The email or password is incorrect.");
    public static readonly Error InvalidEmail = new(ErrorType.Validation, "The provided email is not valid. Please provide another email.");

    public static readonly Error UserNotFound = new(ErrorType.NotFound, "The user was not found.");
    public static readonly Error Forbidden = new(ErrorType.Forbidden, "You don't have permission to access this resource.");
}