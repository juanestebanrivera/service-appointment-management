using Appointments.Domain.Services;

namespace Appointments.Application.Features.Services;

public static class ServiceMappings
{
    extension(Service service)
    {
        public ServiceResponse ToServiceResponse()
        {
            return new ServiceResponse(
                service.Id,
                service.Name,
                service.Description,
                service.Price,
                service.Duration,
                service.IsActive
            );
        }
    }
}