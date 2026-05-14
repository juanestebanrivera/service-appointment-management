namespace Appointments.Domain.SharedKernel.ValueObjects;

public static class EmailErrors
{
    public static readonly Error EmailRequired = new(ErrorType.Validation, "Email is required.");
    public static readonly Error InvalidEmailFormat = new(ErrorType.Validation, "Email format is invalid.");
}