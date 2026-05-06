using Appointments.Domain.SharedKernel;
using Appointments.Domain.SharedKernel.ValueObjects;

namespace Appointments.Domain.Users;

public class User : Entity, IAggregateRoot
{
    public Email Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;

    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }

    private User() { }

    private User(Guid id, Email email, string passwordHash, UserRole role, bool isActive)
        : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        Role = role;
        IsActive = isActive;
    }

    public static Result<User> Register(Email email, string password, IPasswordHasher passwordHasher)
    {
        if (string.IsNullOrWhiteSpace(password))
            return Result<User>.Failure(UserErrors.PasswordRequired);

        const int MINIMUN_PASSWORD_SIZE = 6;

        if (password.Length < MINIMUN_PASSWORD_SIZE)
            return Result<User>.Failure(UserErrors.InvalidPasswordLength);

        string hash = passwordHasher.Hash(password);

        return Result<User>.Success(new User(Guid.NewGuid(), email, hash, UserRole.Client, isActive: true));
    }

    public void Activate() => IsActive = true;

    public void Desactivate() => IsActive = false;
}
