using Appointments.Application.Common.Pagination;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Queries;

public interface IServiceQueryRepository : IRepository<Service>
{
    Task<(IEnumerable<Service> Items, int TotalCount)> GetPagedAsync(PaginationParams pagination, string? searchQuery = null, CancellationToken cancellationToken = default);
}
