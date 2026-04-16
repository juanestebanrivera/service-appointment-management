using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Queries.GetAllClients;

public sealed class GetAllClientsQueryHandler(IClientRepository clientRepository)
    : IQueryHandler<GetAllClientsQuery, IEnumerable<ClientResult>>
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<Result<IEnumerable<ClientResult>>> HandleAsync(GetAllClientsQuery query, CancellationToken cancellationToken = default)
    {
        var clients = await _clientRepository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<ClientResult>>.Success(clients.Select(client => client.ToClientResult()));
    }
}
