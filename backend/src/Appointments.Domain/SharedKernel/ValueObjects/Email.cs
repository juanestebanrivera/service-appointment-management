using System.Text.RegularExpressions;

namespace Appointments.Domain.SharedKernel.ValueObjects;

public partial record Email
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result<Email>.Failure(EmailErrors.EmailRequired);

        if (!EmailFormatRegex().IsMatch(email))
            return Result<Email>.Failure(EmailErrors.InvalidEmailFormat);

        return Result<Email>.Success(new Email(email.ToLowerInvariant()));
    }

    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
    private static partial Regex EmailFormatRegex();
}