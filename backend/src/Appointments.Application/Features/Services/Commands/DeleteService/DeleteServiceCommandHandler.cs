using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Commands.DeleteService;

public sealed class DeleteServiceCommandHandler(
    IServiceRepository serviceRepository,
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<DeleteServiceCommand>
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(DeleteServiceCommand command, CancellationToken cancellationToken = default)
    {
        var service = await _serviceRepository.GetByIdAsync(command.ServiceId, cancellationToken);

        if (service is null)
            return Result.Failure(ServiceApplicationErrors.NotFound);

        if (await _appointmentRepository.ExistsByServiceAsync(service.Id, cancellationToken))
            return Result.Failure(ServiceApplicationErrors.HasAppointments);

        _serviceRepository.Delete(service);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
