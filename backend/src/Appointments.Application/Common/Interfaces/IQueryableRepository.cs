using Appointments.Application.Common.Pagination;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Common.Interfaces;

public interface IQueryableRepository<TEntity> : IRepository<TEntity> where TEntity : IAggregateRoot
{
    Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPagedAsync(PaginationParams pagination, string? searchQuery = null, CancellationToken cancellationToken = default);
}
