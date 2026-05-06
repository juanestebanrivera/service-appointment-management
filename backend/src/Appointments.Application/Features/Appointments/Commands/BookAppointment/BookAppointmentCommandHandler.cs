using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Clients;
using Appointments.Application.Features.Services;
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
            return Result<Guid>.Failure(ClientApplicationErrors.NotFound);

        if (!client.IsActive)
            return Result<Guid>.Failure(ClientErrors.ClientIsInactive);

        var service = await _serviceRepository.GetByIdAsync(command.ServiceId, cancellationToken);

        if (service is null)
            return Result<Guid>.Failure(ServiceApplicationErrors.NotFound);

        if (!service.IsActive)
            return Result<Guid>.Failure(ServiceErrors.ServiceIsInactive);

        var currentTime = _timeProvider.GetUtcNow();
        var endTime = command.StartTime.Add(service.Duration);

        var timeRangeResult = TimeRange.Create(command.StartTime, endTime, currentTime);

        if (timeRangeResult.IsFailure)
            return Result<Guid>.Failure(timeRangeResult.Error);

        var isAvailable = await _appointmentRepository.VerifyAvailabilityAsync(timeRangeResult.Value.StartTime, timeRangeResult.Value.EndTime, excludeAppointmentId: null, cancellationToken);

        if (!isAvailable)
            return Result<Guid>.Failure(AppointmentErrors.TimeSlotUnavailable);

        var appointmentResult = Appointment.Book(
            command.ClientId,
            command.ServiceId,
            timeRangeResult.Value,
            service.Price
        );

        if (appointmentResult.IsFailure)
            return Result<Guid>.Failure(appointmentResult.Error);

        _appointmentRepository.Add(appointmentResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(appointmentResult.Value.Id);
    }
}
