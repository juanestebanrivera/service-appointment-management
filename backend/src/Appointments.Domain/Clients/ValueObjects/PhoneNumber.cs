using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public record PhoneNumber
{
    public string Prefix { get; }
    public string Number { get; }

    private PhoneNumber(string prefix, string number)
    {
        Prefix = prefix;
        Number = number;
    }

    public static Result<PhoneNumber> Create(string prefix, string number)
    {
        if (string.IsNullOrWhiteSpace(prefix))
            return Result<PhoneNumber>.Failure(PhoneNumberErrors.PhonePrefixRequired);

        if (!prefix.StartsWith('+') || prefix.Length < 2 || !prefix[1..].All(char.IsDigit))
            return Result<PhoneNumber>.Failure(PhoneNumberErrors.InvalidPhonePrefix);

        if (string.IsNullOrWhiteSpace(number))
            return Result<PhoneNumber>.Failure(PhoneNumberErrors.PhoneNumberRequired);

        if (!number.All(char.IsDigit) || number.Length < 7)
            return Result<PhoneNumber>.Failure(PhoneNumberErrors.InvalidPhoneNumberFormat);

        return Result<PhoneNumber>.Success(new PhoneNumber(prefix, number));
    }
}