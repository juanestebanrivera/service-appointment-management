using Catalog.Application.Abstractions;
using Catalog.Domain.Services;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Services.Queries;

public record GetServiceByIdQuery(Guid Id);

public class GetServiceByIdQueryHandler(
    IServiceRepository serviceRepository
) : IQueryHandler<GetServiceByIdQuery, ServiceResult>
{
    public async Task<Result<ServiceResult>> HandleAsync(GetServiceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var service = await serviceRepository.GetByIdAsync(query.Id, cancellationToken);
        if (service == null)
            return Result<ServiceResult>.Failure(ServiceErrors.NotFound);

        return Result<ServiceResult>.Success(new ServiceResult(
            service.Id,
            service.Name,
            service.Description,
            service.Price.Amount,
            service.Price.Currency,
            service.Duration.Value
        ));
    }
}