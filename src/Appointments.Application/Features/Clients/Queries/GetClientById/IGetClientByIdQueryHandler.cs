using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Queries.GetClientById;

public interface IGetClientByIdQueryHandler
{
    Task<Result<ClientResponse>> HandleAsync(GetClientByIdQuery query, CancellationToken cancellationToken = default);
}
