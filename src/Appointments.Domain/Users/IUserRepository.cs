using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Users;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAndPasswordAsync(string email, string passwordHash, CancellationToken cancellationToken = default);
    Task<bool> VerifyIfEmailExistsAsync(string email, CancellationToken cancellationToken = default);

    void Add(User user);
    void Update(User user);
}