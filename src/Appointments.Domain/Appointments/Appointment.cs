using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public sealed class Appointment : Entity, IAggregateRoot
{
    public Guid ClientId { get; private set; }
    public Guid ServiceId { get; private set; }
    public decimal PriceAtBooking { get; private set; }
    public TimeRange TimeRange { get; private set; } = null!;
    public AppointmentStatus Status { get; private set; }

    private Appointment() { }
    private Appointment(Guid id, Guid clientId, Guid serviceId, decimal priceAtBooking, TimeRange timeRange, AppointmentStatus status) : base(id)
    {
        ClientId = clientId;
        ServiceId = serviceId;
        TimeRange = timeRange;
        PriceAtBooking = priceAtBooking;
        Status = status;
    }

    public static Result<Appointment> Book(Guid clientId, Guid serviceId, TimeRange timeRange, decimal priceAtBooking)
    {
        if (clientId == Guid.Empty)
            return Result<Appointment>.Failure(AppointmentErrors.ClientIsRequired);

        if (serviceId == Guid.Empty)
            return Result<Appointment>.Failure(AppointmentErrors.ServiceIsRequired);

        if (priceAtBooking <= 0)
            return Result<Appointment>.Failure(AppointmentErrors.PriceAtBookingMustBeGreaterThanZero);

        return Result<Appointment>.Success(new(Guid.NewGuid(), clientId, serviceId, priceAtBooking, timeRange, AppointmentStatus.Pending));
    }

    public Result Reschedule(TimeRange newTimeRange)
    {
        if (Status is not (AppointmentStatus.Pending or AppointmentStatus.Confirmed))
            return Result.Failure(AppointmentErrors.InvalidStatusTransition);

        TimeRange = newTimeRange;

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
}
