using Catalog.Application.Abstractions;
using Catalog.Domain.Services;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Services.Commands;

public record CreateServiceCommand(
    string Name,
    string Description,
    decimal Price,
    string Currency,
    int DurationMinutes
);

public class CreateServiceCommandHandler(
    IUnitOfWork unitOfWork,
    IServiceRepository serviceRepository
) : ICommandHandler<CreateServiceCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(CreateServiceCommand command, CancellationToken cancellationToken = default)
    {
        if (await serviceRepository.ExistsByNameAsync(command.Name, cancellationToken: cancellationToken))
            return Result<Guid>.Failure(ServiceErrors.NameMustBeUnique);

        var price = Money.Create(command.Price, command.Currency);
        if (price.IsFailure)
            return Result<Guid>.Failure(price.Error);

        var duration = DurationMinutes.Create(command.DurationMinutes);
        if (duration.IsFailure)
            return Result<Guid>.Failure(duration.Error);

        var newService = Service.Register(command.Name, command.Description, price.Value, duration.Value);
        if (newService.IsFailure)
            return Result<Guid>.Failure(newService.Error);

        serviceRepository.Add(newService.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(newService.Value.Id);
    }
}