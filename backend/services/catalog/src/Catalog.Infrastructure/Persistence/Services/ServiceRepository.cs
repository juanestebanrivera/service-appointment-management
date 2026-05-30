using Catalog.Domain.Services;
using Catalog.Domain.SharedKernel;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Services;

public class ServiceRepository(CatalogDbContext dbContext) : IServiceRepository
{
    private readonly DbSet<Service> _services = dbContext.Set<Service>();

    public async Task<(IEnumerable<Service> Data, int TotalRecords)> GetPagedAsync(PageParameters pagination, string? search, CancellationToken cancellationToken = default)
    {
        var query = _services.AsNoTracking().AsQueryable();
        query = query.Where(s => s.IsActive);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(s => EF.Functions.ILike(s.Name, $"%{search}%"));
        }

        var totalRecords = await query.CountAsync(cancellationToken);
        var data = await query
            .OrderBy(s => s.Name)
            .Skip((pagination.Page - 1) * pagination.Size)
            .Take(pagination.Size)
            .ToListAsync(cancellationToken);

        return (data, totalRecords);
    }

    public async Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _services.FirstOrDefaultAsync(s => s.Id == id && s.IsActive, cancellationToken);

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default)
        => await _services.AnyAsync(s =>
            s.Name == name
            && s.IsActive
            && (!excludeId.HasValue || s.Id != excludeId.Value),
            cancellationToken);

    public void Add(Service service)
        => _services.Add(service);

    public void Update(Service service)
        => _services.Update(service);
}