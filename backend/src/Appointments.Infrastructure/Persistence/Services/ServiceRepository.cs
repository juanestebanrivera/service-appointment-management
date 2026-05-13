using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Services;

internal sealed class ServiceRepository(ApplicationDbContext dbContext) : IServiceRepository, IQueryableRepository<Service>
{
    private readonly DbSet<Service> _services = dbContext.Set<Service>();

    public async Task<(IEnumerable<Service> Items, int TotalCount)> GetPagedAsync(PaginationParams pagination, string? searchQuery = null, CancellationToken cancellationToken = default)
    {
        var query = _services.AsQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            var pattern = $"%{searchQuery}%";
            query = query.Where(s =>
                EF.Functions.ILike(s.Name, pattern) ||
                (s.Description != null && EF.Functions.ILike(s.Description, pattern)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(s => s.Name)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
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