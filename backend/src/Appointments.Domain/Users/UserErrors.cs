using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Users;

public static class UserErrors
{
    public static readonly Error PasswordRequired = new(ErrorType.Validation, "Password is required.");
    public static readonly Error InvalidPasswordLength = new(ErrorType.Validation, "Password must be at least 6 characters long.");
}