using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public interface IClientRepository : IRepository<Client>
{
    Task<IEnumerable<Client>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    void Add(Client entity);
    void Update(Client entity);
    void Delete(Client entity);
}