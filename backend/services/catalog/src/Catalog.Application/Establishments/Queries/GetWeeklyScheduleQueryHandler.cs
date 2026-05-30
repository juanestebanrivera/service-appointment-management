using Catalog.Application.Abstractions;
using Catalog.Domain.Establishments;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Establishments.Queries;

public record GetWeeklyScheduleQuery();

public class GetWeeklyScheduleQueryHandler(
    IEstablishmentRepository establishmentRepository
) : IQueryHandler<GetWeeklyScheduleQuery, IEnumerable<WeeklySchedule>>
{
    public async Task<Result<IEnumerable<WeeklySchedule>>> HandleAsync(GetWeeklyScheduleQuery query, CancellationToken cancellationToken = default)
    {
        var weeklySchedules = await establishmentRepository.GetWeeklySchedulesAsync(cancellationToken);
        return Result<IEnumerable<WeeklySchedule>>.Success(weeklySchedules);
    }
}