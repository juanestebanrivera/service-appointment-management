using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Queries.GetAllServices;

public sealed class GetAllServicesQueryHandler(IQueryableRepository<Service> serviceRepository)
    : IQueryHandler<GetAllServicesQuery, PagedResult<ServiceResult>>
{
    private readonly IQueryableRepository<Service> _serviceRepository = serviceRepository;

    public async Task<Result<PagedResult<ServiceResult>>> HandleAsync(GetAllServicesQuery query, CancellationToken cancellationToken = default)
    {
        var pagination = new PaginationParams(query.Page, query.PageSize);

        var (items, totalCount) = await _serviceRepository.GetPagedAsync(pagination, query.SearchTerm, cancellationToken);

        var pagedResult = new PagedResult<ServiceResult>
        (
            items.Select(s => s.ToServiceResult()),
            totalCount,
            pagination.Page,
            pagination.PageSize
        );

        return Result<PagedResult<ServiceResult>>.Success(pagedResult);
    }
}
