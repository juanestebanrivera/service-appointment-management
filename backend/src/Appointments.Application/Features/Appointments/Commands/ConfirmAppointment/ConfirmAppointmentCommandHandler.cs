using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.ConfirmAppointment;

public sealed class ConfirmAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IClientRepository clientRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<ConfirmAppointmentCommand>
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(ConfirmAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(command.AppointmentId, cancellationToken);

        if (appointment is null)
            return Result.Failure(AppointmentApplicationErrors.NotFound);

        var client = await _clientRepository.GetByIdAsync(appointment.ClientId, cancellationToken);

        if (client?.UserId != command.CurrentUserId)
            return Result.Failure(AppointmentApplicationErrors.Forbidden);

        var result = appointment.Confirm();

        if (result.IsFailure)
            return Result.Failure(result.Error);

        _appointmentRepository.Update(appointment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
