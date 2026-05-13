using Appointments.Application.Common.Pagination;
using Appointments.Application.Features.Clients.Queries;
using Appointments.Domain.Clients;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Persistence.Clients;

internal sealed class ClientRepository(ApplicationDbContext dbContext) : IClientRepository, IClientQueryRepository
{
    private readonly DbSet<Client> _clients = dbContext.Set<Client>();

    public async Task<(IEnumerable<Client> Items, int TotalCount)> GetPagedAsync(
        PaginationParams pagination,
        string? searchQuery = null,
        bool status = true,
        CancellationToken cancellationToken = default)
    {
        var query = _clients.AsQueryable()
                            .AsNoTracking()
                            .Where(c => c.IsActive == status);

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            var trimmedSearchQuery = searchQuery.Trim();
            var pattern = $"%{trimmedSearchQuery}%";
            var containsDigits = trimmedSearchQuery.Any(char.IsDigit);

            query = query.Where(c =>
                EF.Functions.ILike(c.FirstName.Value, pattern) ||
                EF.Functions.ILike(c.LastName.Value, pattern) ||
                (c.Email != null && EF.Functions.ILike(c.Email.Value, pattern)) ||
                (containsDigits && EF.Functions.ILike(c.Phone.Number, pattern)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.FirstName.Value)
            .ThenBy(c => c.LastName.Value)
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
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
