using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Queries.GetClientById;

public sealed class GetClientByIdQueryHandler(IClientRepository clientRepository)
    : IQueryHandler<GetClientByIdQuery, ClientResult>
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<Result<ClientResult>> HandleAsync(GetClientByIdQuery query, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(query.ClientId, cancellationToken);

        if (client is null)
            return Result<ClientResult>.Failure(ClientApplicationErrors.NotFound);

        return Result<ClientResult>.Success(client.ToClientResult());
    }
}
