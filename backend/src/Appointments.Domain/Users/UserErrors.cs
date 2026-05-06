using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Users;

public static class UserErrors
{
    public static readonly Error PasswordRequired = new("User.PasswordRequired", "Password is required.", ErrorType.Validation);
    public static readonly Error InvalidPasswordLength = new("User.InvalidPasswordLength", "Password must be at least 6 characters long.", ErrorType.Validation);
}