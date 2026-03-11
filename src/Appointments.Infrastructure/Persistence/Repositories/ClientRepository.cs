using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Repositories;

public class ClientRepository(ApplicationDbContext context) : IClientRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        return await _context.Set<Client>().ToListAsync();
    }

    public async Task<Client?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Client>().FirstOrDefaultAsync(c => c.Id == id);
    }
    public async Task AddAsync(Client client)
    {
        await _context.Set<Client>().AddAsync(client);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Client client)
    {
        _context.Set<Client>().Update(client);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Client client)
    {
        _context.Set<Client>().Remove(client);
        await _context.SaveChangesAsync();
    }
}