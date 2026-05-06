using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Queries.GetServiceById;

public sealed class GetServiceByIdQueryHandler(IServiceRepository serviceRepository)
    : IQueryHandler<GetServiceByIdQuery, ServiceResult>
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;

    public async Task<Result<ServiceResult>> HandleAsync(GetServiceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(query.ServiceId, cancellationToken);

        if (service is null)
            return Result<ServiceResult>.Failure(ServiceApplicationErrors.NotFound);

        return Result<ServiceResult>.Success(service.ToServiceResult());
    }
}
