using Appointments.Domain.Clients;

namespace Appointments.Application.Features.Clients.Queries.GetAllClients;

public class GetAllClientsQueryHandler(
    IClientRepository clientRepository
) : IGetAllClientsQueryHandler
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<IEnumerable<ClientResponse>> HandleAsync(GetAllClientsQuery query, CancellationToken cancellationToken = default)
    {
        var clients = await _clientRepository.GetAllAsync(cancellationToken);

        return clients.Select(client => client.ToClientResponse());
    }
}
