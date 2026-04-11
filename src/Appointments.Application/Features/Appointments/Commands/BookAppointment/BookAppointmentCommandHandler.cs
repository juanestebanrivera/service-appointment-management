using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.BookAppointment;

public sealed class BookAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IClientRepository clientRepository,
    IServiceRepository serviceRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider
) : ICommandHandler<BookAppointmentCommand, Guid>
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<Result<Guid>> HandleAsync(BookAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken);

        if (client is null)
            return Result<Guid>.Failure(AppointmentApplicationErrors.ClientNotFound);

        if (!client.IsActive)
            return Result<Guid>.Failure(ClientErrors.ClientIsInactive);

        var service = await _serviceRepository.GetByIdAsync(command.ServiceId, cancellationToken);

        if (service is null)
            return Result<Guid>.Failure(AppointmentApplicationErrors.ServiceNotFound);

        if (!service.IsActive)
            return Result<Guid>.Failure(ServiceErrors.ServiceIsInactive);

        var endTime = command.StartTime.Add(service.Duration);
        var currentTime = _timeProvider.GetUtcNow();

        var appointmentResult = Appointment.Book(
            command.ClientId,
            command.ServiceId,
            command.StartTime,
            endTime,
            service.Price,
            currentTime
        );

        if (appointmentResult.IsFailure)
            return Result<Guid>.Failure(appointmentResult.Error);

        _appointmentRepository.Add(appointmentResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(appointmentResult.Value.Id);
    }
}
