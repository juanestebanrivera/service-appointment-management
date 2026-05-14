namespace Appointments.Domain.SharedKernel;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}