using Appointments.Application.Dtos.Services;
using Appointments.Domain.Entities;

namespace Appointments.Application.Mappings;

public static class ServiceExtension
{
    extension(Service service)
    {
        public ServiceResponse ToServiceResponse()
        {
            return new ServiceResponse(
                service.Id,
                service.Name,
                service.Price,
                service.EstimatedDuration,
                service.IsActive
            );
        }
    }
}