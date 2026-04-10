using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Commands.CreateService;

public sealed class CreateServiceCommandHandler(
    IServiceRepository serviceRepository,
    IUnitOfWork unitOfWork
) : ICreateServiceCommandHandler
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> HandleAsync(CreateServiceCommand command, CancellationToken cancellationToken = default)
    {
        var serviceResult = Service.Create(command.Name, command.Price, command.Duration, command.Description);

        if (serviceResult.IsFailure)
            return Result<Guid>.Failure(serviceResult.Error);

        _serviceRepository.Add(serviceResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(serviceResult.Value.Id);
    }
}
