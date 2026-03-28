namespace Appointments.Application.Features.Clients.Queries.GetAllClients;

public interface IGetAllClientsQueryHandler
{
    Task<IEnumerable<ClientResponse>> HandleAsync(GetAllClientsQuery query, CancellationToken cancellationToken = default);
}
