using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.Services;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.BookAppointment;

public class BookAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IClientRepository clientRepository,
    IServiceRepository serviceRepository,
    IUnitOfWork unitOfWork
) : IBookAppointmentCommandHandler
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IServiceRepository _serviceRepository = serviceRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Guid>> HandleAsync(BookAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        var client = await _clientRepository.GetByIdAsync(command.ClientId, cancellationToken);

        if (client is null)
            return Result<Guid>.Failure(AppointmentApplicationErrors.ClientNotFound);

        var service = await _serviceRepository.GetByIdAsync(command.ServiceId, cancellationToken);

        if (service is null)
            return Result<Guid>.Failure(AppointmentApplicationErrors.ServiceNotFound);

        var endTime = command.StartTime.Add(service.Duration);

        var appointmentResult = Appointment.Book(
            command.ClientId,
            command.ServiceId,
            command.StartTime,
            endTime,
            service.Price
        );

        if (appointmentResult.IsFailure)
            return Result<Guid>.Failure(appointmentResult.Error);

        _appointmentRepository.Add(appointmentResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(appointmentResult.Value.Id);
    }
}
