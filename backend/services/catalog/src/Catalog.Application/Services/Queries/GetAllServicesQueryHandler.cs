using Catalog.Application.Abstractions;
using Catalog.Domain.Services;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Services.Queries;

public record GetAllServicesQuery(int Page, int Size, string? Search = null);

public class GetAllServicesQueryHandler(
    IServiceRepository serviceRepository
) : IQueryHandler<GetAllServicesQuery, PagedList<ServiceResult>>
{
    public async Task<Result<PagedList<ServiceResult>>> HandleAsync(GetAllServicesQuery query, CancellationToken cancellationToken = default)
    {
        var pagination = new PageParameters(query.Page, query.Size);
        var (data, totalRecords) = await serviceRepository.GetPagedAsync(pagination, query.Search, cancellationToken);

        var services = data.Select(s => new ServiceResult(
            s.Id,
            s.Name,
            s.Description,
            s.Price.Amount,
            s.Price.Currency,
            s.Duration.Value
        ));

        return Result<PagedList<ServiceResult>>.Success(new PagedList<ServiceResult>(
            services,
            pagination.Page,
            pagination.Size,
            totalRecords
        ));
    }
}