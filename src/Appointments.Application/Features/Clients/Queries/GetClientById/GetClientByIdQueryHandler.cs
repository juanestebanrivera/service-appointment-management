using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Queries.GetClientById;

public sealed class GetClientByIdQueryHandler(IClientRepository clientRepository) : IGetClientByIdQueryHandler
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<Result<ClientResponse>> HandleAsync(GetClientByIdQuery query, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(query.ClientId, cancellationToken);

        if (client is null)
            return Result<ClientResponse>.Failure(ClientApplicationErrors.NotFound);

        return Result<ClientResponse>.Success(client.ToClientResponse());
    }
}
