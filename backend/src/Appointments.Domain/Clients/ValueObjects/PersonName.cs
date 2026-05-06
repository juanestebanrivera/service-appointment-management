using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public record PersonName
{
    public string Value { get; }

    private PersonName(string value)
    {
        Value = value;
    }

    public static Result<PersonName> Create(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Result<PersonName>.Failure(PersonNameErrors.IsRequired(fieldName));

        if (value.Any(char.IsNumber))
            return Result<PersonName>.Failure(PersonNameErrors.CannotContainNumbers(fieldName));

        if (value.Length < 2)
            return Result<PersonName>.Failure(PersonNameErrors.MustBeAtLeastTwoCharacters(fieldName));

        return Result<PersonName>.Success(new PersonName(value));
    }
}