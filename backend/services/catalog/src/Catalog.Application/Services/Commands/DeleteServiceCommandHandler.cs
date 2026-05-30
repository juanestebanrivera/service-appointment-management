using Catalog.Application.Abstractions;
using Catalog.Domain.Services;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Services.Commands;

public record DeleteServiceCommand(Guid Id);

public class DeleteServiceCommandHandler(
    IUnitOfWork unitOfWork,
    IServiceRepository serviceRepository
) : ICommandHandler<DeleteServiceCommand>
{
    public async Task<Result> HandleAsync(DeleteServiceCommand command, CancellationToken cancellationToken = default)
    {
        var service = await serviceRepository.GetByIdAsync(command.Id, cancellationToken);
        if (service == null)
            return Result.Failure(ServiceErrors.NotFound);

        // Instead of deleting the service, we mark it as inactive to preserve historical data.
        service.Deactivate();
        serviceRepository.Update(service);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}