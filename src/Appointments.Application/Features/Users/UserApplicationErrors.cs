using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Users;

public static class UserApplicationErrors
{
    public static readonly Error InvalidCredentials = new("InvalidCredentials", "The provided email or password is incorrect.", ErrorType.Unauthorized);
    public static readonly Error InvalidEmail = new("InvalidEmail", "The provided email is not valid. Please provide another email.", ErrorType.Validation);
}