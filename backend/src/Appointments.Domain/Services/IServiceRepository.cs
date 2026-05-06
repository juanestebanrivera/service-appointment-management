using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Services;

public interface IServiceRepository : IRepository<Service>
{
    Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    void Add(Service entity);
    void Update(Service entity);
    void Delete(Service entity);
}
