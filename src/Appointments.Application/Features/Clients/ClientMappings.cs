using Appointments.Domain.Clients;

namespace Appointments.Application.Features.Clients;

public static class ClientMappings
{
    extension(Client client)
    {
        public ClientResult ToClientResult()
        {
            return new ClientResult(
                client.Id,
                client.FirstName.Value,
                client.LastName.Value,
                client.Phone.Prefix,
                client.Phone.Number,
                client.Email?.Value,
                client.IsActive
            );
        }
    }
}