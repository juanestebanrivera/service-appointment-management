using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Queries.GetServiceById;

public sealed class GetServiceByIdQueryHandler(IServiceRepository serviceRepository)
    : IQueryHandler<GetServiceByIdQuery, ServiceResponse>
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;

    public async Task<Result<ServiceResponse>> HandleAsync(GetServiceByIdQuery query, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(query.ServiceId, cancellationToken);

        if (service is null)
            return Result<ServiceResponse>.Failure(ServiceApplicationErrors.NotFound);

        return Result<ServiceResponse>.Success(service.ToServiceResponse());
    }
}
