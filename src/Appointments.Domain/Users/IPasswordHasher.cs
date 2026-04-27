namespace Appointments.Domain.Users;

public interface IPasswordHasher
{
    string Hash(string password);
}