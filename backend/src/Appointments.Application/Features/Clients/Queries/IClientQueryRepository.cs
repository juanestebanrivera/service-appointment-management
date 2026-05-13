using Appointments.Application.Common.Pagination;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Queries;

public interface IClientQueryRepository : IRepository<Client>
{
    Task<(IEnumerable<Client> Items, int TotalCount)> GetPagedAsync(
        PaginationParams pagination,
        string? searchQuery = null,
        bool status = true,
        CancellationToken cancellationToken = default);
}
