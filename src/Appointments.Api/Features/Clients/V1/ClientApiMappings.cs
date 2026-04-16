using Appointments.Api.Features.Clients.V1.Contracts;
using Appointments.Application.Features.Clients;

namespace Appointments.Api.Features.Clients.V1;

public static class ClientApiMappings
{
    extension(ClientResult result)
    {
        public ClientApiResponse ToClientApiResponse()
        {
            return new ClientApiResponse(
                result.Id,
                result.FirstName,
                result.LastName,
                result.PhonePrefix,
                result.PhoneNumber,
                result.Email,
                result.IsActive
            );
        }
    }
}