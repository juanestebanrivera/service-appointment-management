namespace Appointments.Application.Features.Clients.Queries.GetAllClients;

public interface IGetAllClientsQueryHandler
{
    Task<IEnumerable<ClientResponse>> HandleAsync(CancellationToken cancellationToken = default);
}
