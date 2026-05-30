using Catalog.Application.Abstractions;
using Catalog.Domain.Establishments;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Establishments.Commands;

public record UpdateEstablishmentCommand(
    Guid Id,
    string ComercialName,
    string Address,
    string PhoneNumber
);

public class UpdateEstablishmentCommandHandler(
    IUnitOfWork unitOfWork,
    IEstablishmentRepository establishmentRepository
) : ICommandHandler<UpdateEstablishmentCommand>
{
    public async Task<Result> HandleAsync(UpdateEstablishmentCommand command, CancellationToken cancellationToken = default)
    {
        var establishment = await establishmentRepository.GetByIdAsync(command.Id, cancellationToken);
        if (establishment == null)
            return Result.Failure(EstablishmentErrors.NotFound);

        var updateResult = establishment.UpdateBasicInfo(command.ComercialName, command.Address, command.PhoneNumber);
        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error);

        establishmentRepository.Update(establishment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}