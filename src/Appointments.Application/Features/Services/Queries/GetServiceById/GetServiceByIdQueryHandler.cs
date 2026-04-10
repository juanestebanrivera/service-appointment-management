using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler(IServiceRepository serviceRepository) : IGetServiceByIdQueryHandler
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;

    public async Task<Result<ServiceResponse>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(id, cancellationToken);

        if (service is null)
            return Result<ServiceResponse>.Failure(ServiceApplicationErrors.NotFound);

        return Result<ServiceResponse>.Success(service.ToServiceResponse());
    }
}
