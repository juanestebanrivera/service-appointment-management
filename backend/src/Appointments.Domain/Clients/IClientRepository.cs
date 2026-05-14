using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Clients;

public interface IClientRepository : IRepository<Client>
{
    void Add(Client entity);
    void Update(Client entity);
    void Delete(Client entity);
}