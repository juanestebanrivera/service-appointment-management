namespace Catalog.Domain.Establishments;

public interface IEstablishmentRepository
{
    Task<Establishment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    void Update(Establishment establishment);
}