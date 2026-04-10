using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Queries.GetClientById;

public class GetClientByIdQueryHandler(IClientRepository clientRepository) : IGetClientByIdQueryHandler
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<Result<ClientResponse>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(id, cancellationToken);

        if (client is null)
            return Result<ClientResponse>.Failure(ClientApplicationErrors.NotFound);

        return Result<ClientResponse>.Success(client.ToClientResponse());
    }
}
