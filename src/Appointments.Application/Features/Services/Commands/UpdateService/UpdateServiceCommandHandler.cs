using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Commands.UpdateService;

public sealed class UpdateServiceCommandHandler(
    IServiceRepository serviceRepository,
    IUnitOfWork unitOfWork
) : IUpdateServiceCommandHandler
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(UpdateServiceCommand command, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(command.ServiceId, cancellationToken);

        if (service is null)
            return Result.Failure(ServiceApplicationErrors.NotFound);

        var updateInfoResult = service.UpdateInformation(command.Name, command.Description);

        if (updateInfoResult.IsFailure)
            return Result.Failure(updateInfoResult.Error);

        var priceResult = service.AdjustPrice(command.Price);

        if (priceResult.IsFailure)
            return Result.Failure(priceResult.Error);

        var durationResult = service.ChangeDuration(command.Duration);

        if (durationResult.IsFailure)
            return Result.Failure(durationResult.Error);

        if (command.IsActive)
        {
            service.Activate();
        }
        else
        {
            service.Deactivate();
        }

        _serviceRepository.Update(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
