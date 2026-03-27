using Appointments.Domain.Clients;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Clients;

internal sealed class ClientRepository(ApplicationDbContext dbContext) : IClientRepository
{
    private readonly DbSet<Client> _clients = dbContext?.Set<Client>() ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _clients.ToListAsync(cancellationToken);
    }

    public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _clients.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }
    
    public void Add(Client client)
    {
        _clients.Add(client);
    }

    public void Update(Client client)
    {
        _clients.Update(client);
    }

    public void Delete(Client client)
    {
        _clients.Remove(client);
    }
}