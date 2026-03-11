using Appointments.Application.Dtos.Clients;
using Appointments.Domain.Entities;

namespace Appointments.Application.Mappings;

public static class ClientExtension
{
    extension(Client client)
    {
        public ClientResponse ToClientResponse()
        {
            return new ClientResponse(
                client.Id,
                client.Name,
                client.Email,
                client.PhoneNumber,
                client.DateOfBirth
            );
        }
    }
}