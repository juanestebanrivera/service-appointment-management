using Appointments.Domain.Services;

namespace Appointments.Application.Features.Services.Queries.GetAllServices;

public sealed class GetAllServicesQueryHandler(IServiceRepository serviceRepository) : IGetAllServicesQueryHandler
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;

    public async Task<IEnumerable<ServiceResponse>> HandleAsync(GetAllServicesQuery query, CancellationToken cancellationToken = default)
    {
        var services = await _serviceRepository.GetAllAsync(cancellationToken);

        return services.Select(service => service.ToServiceResponse());
    }
}
