using Appointments.Api.Features.Services.V1.Contracts;
using Appointments.Application.Features.Services;

namespace Appointments.Api.Features.Services.V1;

public static class ServiceApiMappings
{
    extension(ServiceResult service)
    {
        public ServiceApiResponse ToServiceApiResponse()
        {
            return new ServiceApiResponse(
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