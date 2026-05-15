using Appointments.Domain.SharedKernel;
using Appointments.Domain.SharedKernel.ValueObjects;

namespace Appointments.Domain.Clients;

public interface IClientRepository : IRepository<Client>
{
    void Add(Client entity);
    void Update(Client entity);
    void Delete(Client entity);
    Task<bool> ExistsByPhoneAsync(PhoneNumber phone, Guid? excludeClientId = null, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(Email email, Guid? excludeClientId = null, CancellationToken cancellationToken = default);
}