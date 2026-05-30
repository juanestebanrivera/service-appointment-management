namespace Catalog.Domain.Establishments;

public interface IEstablishmentRepository
{
    Task<Establishment?> GetAsync(CancellationToken cancellationToken);
    Task<Establishment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<WeeklySchedule>> GetWeeklySchedulesAsync(CancellationToken cancellationToken);

    void Update(Establishment establishment);
}