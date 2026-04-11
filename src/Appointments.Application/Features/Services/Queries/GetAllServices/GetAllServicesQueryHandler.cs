using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Queries.GetAllServices;

public sealed class GetAllServicesQueryHandler(IServiceRepository serviceRepository) 
    : IQueryHandler<GetAllServicesQuery, IEnumerable<ServiceResponse>>
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;

    public async Task<Result<IEnumerable<ServiceResponse>>> HandleAsync(GetAllServicesQuery query, CancellationToken cancellationToken = default)
    {
        var services = await _serviceRepository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<ServiceResponse>>.Success(services.Select(service => service.ToServiceResponse()));
    }
}
