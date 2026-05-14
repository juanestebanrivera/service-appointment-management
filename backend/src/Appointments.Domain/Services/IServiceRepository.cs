using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Services;

public interface IServiceRepository : IRepository<Service>
{
    void Add(Service entity);
    void Update(Service entity);
    void Delete(Service entity);
}
