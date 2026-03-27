using Appointments.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Services;

internal sealed class ServiceRepository(ApplicationDbContext dbContext) : IServiceRepository
{
    private readonly DbSet<Service> _services = dbContext?.Set<Service>() ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _services.ToListAsync(cancellationToken);
    }

    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _services.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public void Add(Service service)
    {
        _services.Add(service);
    }

    public void Update(Service service)
    {
        _services.Update(service);
    }

    public void Delete(Service service)
    {
        _services.Remove(service);
    }
}