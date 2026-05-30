using Catalog.Application.Abstractions;
using Catalog.Domain.Establishments;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Establishments.Commands;

public record WeeklyScheduleDto(DayOfWeek Day, TimeSpan OpeningTime, TimeSpan ClosingTime);
public record UpdateWeeklyScheduleCommand(Guid EstablishmentId, IEnumerable<WeeklyScheduleDto> WeeklySchedules);

public class UpdateWeeklyScheduleCommandHandler(
    IUnitOfWork unitOfWork,
    IEstablishmentRepository establishmentRepository
) : ICommandHandler<UpdateWeeklyScheduleCommand>
{
    public async Task<Result> HandleAsync(UpdateWeeklyScheduleCommand command, CancellationToken cancellationToken = default)
    {
        var establishment = await establishmentRepository.GetByIdAsync(command.EstablishmentId, cancellationToken);
        if (establishment == null)
            return Result.Failure(EstablishmentErrors.NotFound);

        var weeklySchedules = command.WeeklySchedules.Select(ws => WeeklySchedule.Create(ws.Day, ws.OpeningTime, ws.ClosingTime));
        if (weeklySchedules.Any(ws => ws.IsFailure))
        {
            var firstError = weeklySchedules.First(ws => ws.IsFailure).Error;
            return Result.Failure(firstError);
        }

        var updateResult = establishment.UpdateWeeklySchedules(weeklySchedules.Select(ws => ws.Value).ToList());
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);

        establishmentRepository.Update(establishment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}