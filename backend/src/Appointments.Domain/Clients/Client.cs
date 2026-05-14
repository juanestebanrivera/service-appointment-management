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

    public Guid UserId { get; private set; }

    private Client() { }

    private Client(Guid id, PersonName firstName, PersonName lastName, PhoneNumber phone, Email? email, bool isActive, Guid userId)
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Phone = phone;
        Email = email;
        IsActive = isActive;
        UserId = userId;
    }

    public static Result<Client> Register(PersonName firstName, PersonName lastName, PhoneNumber phone, Guid userId, Email? email = null)
    {
        return Result<Client>.Success(new(Guid.NewGuid(), firstName, lastName, phone, email, true, userId));
    }

    public void UpdateContactInfo(PersonName firstName, PersonName lastName, Email? email, PhoneNumber phone)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
    }

    public void Activate() => IsActive = true;

    public void Deactivate() => IsActive = false;
}