using Appointments.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Users;

internal class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    private readonly DbSet<User> _users = dbContext.Set<User>();

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _users.FirstOrDefaultAsync(user => user.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _users.FirstOrDefaultAsync(user => user.Email.Value == email, cancellationToken);
    }

    public async Task<bool> VerifyIfEmailExistsAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _users.AnyAsync(user => user.Email.Value == email, cancellationToken);
    }

    public void Add(User user)
    {
        _users.Add(user);
    }

    public void Update(User user)
    {
        _users.Update(user);
    }
}