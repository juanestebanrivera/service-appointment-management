using Appointments.Application.Interfaces.Repositories;
using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Repositories;

public class ServiceRepository(ApplicationDbContext context) : IServiceRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<IEnumerable<Service>> GetAllAsync()
    {
        return await _context.Set<Service>().ToListAsync();
    }

    public async Task<Service?> GetByIdAsync(Guid id)
    {
        return await _context.Set<Service>().FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task AddAsync(Service service)
    {
        await _context.Set<Service>().AddAsync(service);
        await _context.SaveChangesAsync();
    }

    public Task UpdateAsync(Service service)
    {
        _context.Set<Service>().Update(service);
        return _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Service service)
    {
        _context.Set<Service>().Remove(service);
        await _context.SaveChangesAsync();
    }
}