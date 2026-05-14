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

    /// <summary>
    /// Reschedules the appointment to a new time range and resets its status to Pending, since the new schedule requires re-confirmation.
    /// </summary>
    public Result Reschedule(TimeRange newTimeRange)
    {
        if (!CanTransitionTo([AppointmentStatus.Pending, AppointmentStatus.Confirmed]))
            return Result.Failure(AppointmentErrors.InvalidStatusTransition);

        TimeRange = newTimeRange;
        Status = AppointmentStatus.Pending;

        return Result.Success();
    }

    public Result Confirm() => ChangeStatus(AppointmentStatus.Confirmed, [AppointmentStatus.Pending]);

    public Result Cancel() => ChangeStatus(AppointmentStatus.Cancelled, [AppointmentStatus.Pending, AppointmentStatus.Confirmed]);

    public Result Complete() => ChangeStatus(AppointmentStatus.Completed, [AppointmentStatus.Confirmed]);

    public Result MarkAsNoShow() => ChangeStatus(AppointmentStatus.NoShow, [AppointmentStatus.Confirmed]);

    private Result ChangeStatus(AppointmentStatus next, AppointmentStatus[] allowed)
    {
        if (!CanTransitionTo(allowed))
            return Result.Failure(AppointmentErrors.InvalidStatusTransition);

        Status = next;

        return Result.Success();
    }

    private bool CanTransitionTo(AppointmentStatus[] allowed) => allowed.Contains(Status);
}
