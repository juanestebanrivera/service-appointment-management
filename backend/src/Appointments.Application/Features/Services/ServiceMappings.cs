using Appointments.Domain.Services;

namespace Appointments.Application.Features.Services;

public static class ServiceMappings
{
    extension(Service service)
    {
        public ServiceResult ToServiceResult()
        {
            return new ServiceResult(
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