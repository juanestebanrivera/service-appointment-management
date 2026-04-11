using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public sealed class Appointment : Entity, IAggregateRoot
{
    public Guid ClientId { get; private set; }
    public Guid ServiceId { get; private set; }
    public decimal PriceAtBooking { get; private set; }
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; }
    public AppointmentStatus Status { get; private set; }

    private Appointment() {}
    private Appointment(Guid id, Guid clientId, Guid serviceId, decimal priceAtBooking, DateTimeOffset startTime, DateTimeOffset endTime, AppointmentStatus status) : base(id)
    {
        ClientId = clientId;
        ServiceId = serviceId;
        StartTime = startTime;
        EndTime = endTime;
        PriceAtBooking = priceAtBooking;
        Status = status;
    }

    public static Result<Appointment> Book(Guid clientId, Guid serviceId, DateTimeOffset startTime, DateTimeOffset endTime, decimal priceAtBooking, DateTimeOffset currentTime)
    {
        if (clientId == Guid.Empty)
            return Result<Appointment>.Failure(AppointmentErrors.ClientIsRequired);

        if (serviceId == Guid.Empty)
            return Result<Appointment>.Failure(AppointmentErrors.ServiceIsRequired);

        var timeValidationResult = ValidateTime(startTime, endTime, currentTime);

        if (timeValidationResult.IsFailure)
            return Result<Appointment>.Failure(timeValidationResult.Error);

        if (priceAtBooking <= 0)
            return Result<Appointment>.Failure(AppointmentErrors.PriceAtBookingMustBeGreaterThanZero);

        return Result<Appointment>.Success(new(Guid.NewGuid(), clientId, serviceId, priceAtBooking, startTime, endTime, AppointmentStatus.Pending));
    }

    public Result Reschedule(DateTimeOffset newStartTime, DateTimeOffset currentTime)
    {
        if (Status is not (AppointmentStatus.Pending or AppointmentStatus.Confirmed))
            return Result.Failure(AppointmentErrors.InvalidStatusTransition);

        var newEndTime = newStartTime.Add(EndTime - StartTime);
        var timeValidationResult = ValidateTime(newStartTime, newEndTime, currentTime);

        if (timeValidationResult.IsFailure)
            return Result.Failure(timeValidationResult.Error);

        StartTime = newStartTime;
        EndTime = newEndTime;

        return Result.Success();
    }

    public Result Confirm()
    {
        if (Status != AppointmentStatus.Pending)
            return Result.Failure(AppointmentErrors.InvalidStatusTransition);

        Status = AppointmentStatus.Confirmed;

        return Result.Success();
    }

    public Result Cancel()
    {
        if (Status is not (AppointmentStatus.Pending or AppointmentStatus.Confirmed))
            return Result.Failure(AppointmentErrors.InvalidStatusTransition);

        Status = AppointmentStatus.Cancelled;

        return Result.Success();
    }

    public Result Complete()
    {
        if (Status != AppointmentStatus.Confirmed)
            return Result.Failure(AppointmentErrors.InvalidStatusTransition);

        Status = AppointmentStatus.Completed;

        return Result.Success();
    }

    public Result MarkAsNoShow()
    {
        if (Status != AppointmentStatus.Confirmed)
            return Result.Failure(AppointmentErrors.InvalidStatusTransition);

        Status = AppointmentStatus.NoShow;

        return Result.Success();
    }
    
    private static Result ValidateTime(DateTimeOffset startTime, DateTimeOffset endTime, DateTimeOffset currentTime)
    {
        if (startTime < currentTime)
            return Result.Failure(AppointmentErrors.TimeCannotBeInThePast);

        if (endTime <= startTime)
            return Result.Failure(AppointmentErrors.EndTimeMustBeAfterStartTime);

        if (endTime <= startTime.AddMinutes(5))
            return Result.Failure(AppointmentErrors.TimeMustBeMoreThanFiveMinutes);

        if (endTime > startTime.AddDays(1))
            return Result.Failure(AppointmentErrors.TimeMustBeLessThanOneDay);

        return Result.Success();
    }
}
