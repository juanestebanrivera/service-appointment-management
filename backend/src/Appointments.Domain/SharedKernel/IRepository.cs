namespace Appointments.Domain.SharedKernel;

public interface IRepository<TEntity> where TEntity : IAggregateRoot
{
}