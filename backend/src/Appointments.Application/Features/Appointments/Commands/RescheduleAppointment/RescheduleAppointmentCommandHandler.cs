using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;

public sealed class RescheduleAppointmentCommandHandler(
    IAppointmentRepository appointmentRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider
) : ICommandHandler<RescheduleAppointmentCommand>
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly TimeProvider _timeProvider = timeProvider;

    public async Task<Result> HandleAsync(RescheduleAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(command.AppointmentId, cancellationToken);

        if (appointment is null)
            return Result.Failure(AppointmentApplicationErrors.NotFound);

        var currentTime = _timeProvider.GetUtcNow();
        var newEndTime = command.NewStartTime.Add(appointment.TimeRange.Duration);

        var timeRangeResult = TimeRange.Create(command.NewStartTime, newEndTime, currentTime);

        if (timeRangeResult.IsFailure)
            return Result.Failure(timeRangeResult.Error);

        var isAvailable = await _appointmentRepository.VerifyAvailabilityAsync(timeRangeResult.Value.StartTime, timeRangeResult.Value.EndTime, excludeAppointmentId: appointment.Id, cancellationToken);

        if (!isAvailable)
            return Result.Failure(AppointmentErrors.TimeSlotUnavailable);

        var result = appointment.Reschedule(timeRangeResult.Value);

        if (result.IsFailure)
            return Result.Failure(result.Error);

        _appointmentRepository.Update(appointment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
