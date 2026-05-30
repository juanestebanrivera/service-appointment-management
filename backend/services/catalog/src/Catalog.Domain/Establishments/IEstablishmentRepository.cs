namespace Catalog.Domain.Establishments;

public interface IEstablishmentRepository
{
    Task<Establishment?> GetAsync(CancellationToken cancellationToken);
    Task<Establishment?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<WeeklySchedule>> GetWeeklySchedulesAsync(CancellationToken cancellationToken);

    void Update(Establishment establishment);
}