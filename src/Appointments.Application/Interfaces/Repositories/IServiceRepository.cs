using Appointments.Domain.Entities;

namespace Appointments.Application.Interfaces.Repositories;

public interface IServiceRepository
{
    Task<IEnumerable<Service>> GetAllAsync();
    Task<Service?> GetByIdAsync(Guid id);
    Task AddAsync(Service service);
    Task UpdateAsync(Service service);
    Task DeleteAsync(Service service);
}
