using Catalog.Domain.Establishments;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Persistence.Establishments;

public class EstablishmentRepository(CatalogDbContext dbContext) : IEstablishmentRepository
{
    private readonly DbSet<Establishment> _establishments = dbContext.Set<Establishment>();

    public Task<Establishment?> GetAsync(CancellationToken cancellationToken)
        => _establishments.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

    public async Task<Establishment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        => await _establishments.FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<WeeklySchedule>> GetWeeklySchedulesAsync(CancellationToken cancellationToken)
    {
        var weeklySchedules = await _establishments
            .AsNoTracking()
            .Select(e => e.WeeklySchedules)
            .FirstOrDefaultAsync(cancellationToken);

        return weeklySchedules ?? [];
    }

    public void Update(Establishment establishment)
        => _establishments.Update(establishment);
}