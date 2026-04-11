using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.ConfirmAppointment;

public sealed class ConfirmAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<ConfirmAppointmentCommand>
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> HandleAsync(ConfirmAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(command.AppointmentId, cancellationToken);

        if (appointment is null)
            return Result.Failure(AppointmentApplicationErrors.NotFound);

        var result = appointment.Confirm();

        if (result.IsFailure)
            return Result.Failure(result.Error);

        _appointmentRepository.Update(appointment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
