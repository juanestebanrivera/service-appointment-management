using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public sealed class Client : Entity, IAggregateRoot
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public PhoneNumber Phone { get; private set; } = null!;
    public Email? Email { get; private set; }
    public bool IsActive { get; private set; }

    private Client() {}

    private Client(Guid id, string firstName, string lastName, PhoneNumber phone, Email? email, bool isActive) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        IsActive = isActive;
    }

    public static Result<Client> Register(string firstName, string lastName, PhoneNumber phone, Email? email = null, bool? isActive = null)
    {
        var validationNamesResult = ValidateNames(firstName, lastName);

        if (validationNamesResult.IsFailure)
            return Result<Client>.Failure(validationNamesResult.Error);

        return Result<Client>.Success(new (Guid.NewGuid(), firstName, lastName, phone, email, isActive ?? true));
    }

    public Result ChangeName(string newFirstName, string newLastName)
    {
        var validationNamesResult = ValidateNames(newFirstName, newLastName);

        if (validationNamesResult.IsFailure)
            return Result.Failure(validationNamesResult.Error);

        FirstName = newFirstName;
        LastName = newLastName;
        
        return Result.Success();
    }

    public void ChangeEmail(Email? newEmail) => Email = newEmail;

    public void ChangePhoneNumber(PhoneNumber newPhone) => Phone = newPhone;

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;

    private static Result ValidateNames(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return Result.Failure(ClientErrors.FirstNameIsRequired);
        
        if (firstName.Any(char.IsNumber))
            return Result.Failure(ClientErrors.FirstNameCannotContainNumbers);

        if (firstName.Length < 2)
            return Result.Failure(ClientErrors.FirstNameMustBeAtLeastTwoCharacters);

        if (string.IsNullOrWhiteSpace(lastName))
            return Result.Failure(ClientErrors.LastNameIsRequired);

        if (lastName.Any(char.IsNumber))
            return Result.Failure(ClientErrors.LastNameCannotContainNumbers);

        if (lastName.Length < 2)
            return Result.Failure(ClientErrors.LastNameMustBeAtLeastTwoCharacters);

        return Result.Success();
    }
}