using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public static class AppointmentErrors
{
    public static readonly Error ClientIsRequired = new(ErrorType.Validation, "Client is required");

    public static readonly Error ServiceIsRequired = new(ErrorType.Validation, "Service is required");
    public static readonly Error PriceAtBookingMustBeGreaterThanZero = new(ErrorType.Validation, "Price at booking must be greater than zero");

    public static readonly Error InvalidStatusTransition = new(ErrorType.Conflict, "Invalid status transition");
    public static readonly Error TimeSlotUnavailable = new(ErrorType.Conflict, "Time slot is unavailable");
    public static readonly Error ClientAlreadyHasActiveAppointment = new(ErrorType.Conflict, "Client already has a pending or confirmed appointment");
}