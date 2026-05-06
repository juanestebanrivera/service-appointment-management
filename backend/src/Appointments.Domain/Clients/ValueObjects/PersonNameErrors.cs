using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public static class PersonNameErrors
{
    public static Error IsRequired(string fieldName) => new($"{fieldName}.Required", $"{fieldName} is required.", ErrorType.Validation);
    public static Error CannotContainNumbers(string fieldName) => new($"{fieldName}.CannotContainNumbers", $"{fieldName} cannot contain numbers.", ErrorType.Validation);
    public static Error MustBeAtLeastTwoCharacters(string fieldName) => new($"{fieldName}.MustBeAtLeastTwoCharacters", $"{fieldName} must be at least 2 characters long.", ErrorType.Validation);
}