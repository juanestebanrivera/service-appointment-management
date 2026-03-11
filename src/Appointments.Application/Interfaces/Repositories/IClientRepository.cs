using Appointments.Domain.Entities;

namespace Appointments.Application.Interfaces.Repositories;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllAsync();
    Task<Client?> GetByIdAsync(Guid id);
    Task AddAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(Guid id);
}