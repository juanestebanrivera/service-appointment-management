using Appointments.Domain.Clients;

namespace Appointments.Application.Features.Clients;

public static class ClientMappings
{
    extension(Client client)
    {
        public ClientResponse ToClientResponse()
        {
            return new ClientResponse(
                client.Id,
                client.FirstName,
                client.LastName,
                client.Phone.Prefix,
                client.Phone.Number,
                client.Email?.Value,
                client.IsActive
            );
        }
    }
}