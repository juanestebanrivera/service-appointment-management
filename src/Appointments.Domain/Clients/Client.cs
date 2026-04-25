using Appointments.Domain.SharedKernel;
using Appointments.Domain.SharedKernel.ValueObjects;

namespace Appointments.Domain.Clients;

public sealed class Client : Entity, IAggregateRoot
{
    public PersonName FirstName { get; private set; } = null!;
    public PersonName LastName { get; private set; } = null!;
    public PhoneNumber Phone { get; private set; } = null!;
    public Email? Email { get; private set; }
    public bool IsActive { get; private set; }

    private Client() { }

    private Client(Guid id, PersonName firstName, PersonName lastName, PhoneNumber phone, Email? email, bool isActive) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        IsActive = isActive;
    }

    public static Result<Client> Register(PersonName firstName, PersonName lastName, PhoneNumber phone, Email? email = null)
    {
        return Result<Client>.Success(new(Guid.NewGuid(), firstName, lastName, phone, email, true));
    }

    public void ChangeName(PersonName newFirstName, PersonName newLastName)
    {
        FirstName = newFirstName;
        LastName = newLastName;
    }

    public void ChangeEmail(Email? newEmail) => Email = newEmail;

    public void ChangePhoneNumber(PhoneNumber newPhone) => Phone = newPhone;

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}