using Catalog.Application.Abstractions;
using Catalog.Domain.Services;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Services.Commands;

public record DeactivateServiceCommand(Guid Id);

public class DeactivateServiceCommandHandler(
    IUnitOfWork unitOfWork,
    IServiceRepository serviceRepository
) : ICommandHandler<DeactivateServiceCommand>
{
    public async Task<Result> HandleAsync(DeactivateServiceCommand command, CancellationToken cancellationToken = default)
    {
        var service = await serviceRepository.GetByIdAsync(command.Id, cancellationToken);
        if (service == null)
            return Result.Failure(ServiceErrors.NotFound);

        service.Deactivate();
        serviceRepository.Update(service);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}