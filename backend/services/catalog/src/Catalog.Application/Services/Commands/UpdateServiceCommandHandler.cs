using Catalog.Application.Abstractions;
using Catalog.Domain.Services;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Services.Commands;

public record UpdateServiceCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    int Duration
);

public class UpdateServiceCommandHandler(
    IUnitOfWork unitOfWork,
    IServiceRepository serviceRepository
) : ICommandHandler<UpdateServiceCommand>
{
    public async Task<Result> HandleAsync(UpdateServiceCommand command, CancellationToken cancellationToken = default)
    {
        var service = await serviceRepository.GetByIdAsync(command.Id, cancellationToken);
        if (service == null)
            return Result.Failure(ServiceErrors.NotFound);

        if (await serviceRepository.ExistsByNameAsync(command.Name, excludeId: command.Id, cancellationToken: cancellationToken))
            return Result.Failure(ServiceErrors.NameMustBeUnique);

        var price = Money.Create(command.Price, command.Currency);
        if (price.IsFailure)
            return Result.Failure(price.Error);

        var duration = DurationMinutes.Create(command.Duration);
        if (duration.IsFailure)
            return Result.Failure(duration.Error);

        var updateResult = service.UpdateInformation(command.Name, command.Description, price.Value, duration.Value);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);

        serviceRepository.Update(service);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}