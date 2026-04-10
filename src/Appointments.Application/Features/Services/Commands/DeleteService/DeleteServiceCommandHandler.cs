using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Commands.DeleteService;

public sealed class DeleteServiceCommandHandler(
    IServiceRepository serviceRepository,
    IUnitOfWork unitOfWork
) : IDeleteServiceCommandHandler
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(DeleteServiceCommand command, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(command.ServiceId, cancellationToken);

        if (service is null)
            return Result.Failure(ServiceApplicationErrors.NotFound);

        _serviceRepository.Delete(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
