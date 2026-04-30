using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public static class AppointmentErrors
{
    public static readonly Error ClientIsRequired = new("Appointment.ClientIsRequired", "Client is required", ErrorType.Validation);

    public static readonly Error ServiceIsRequired = new("Appointment.ServiceIsRequired", "Service is required", ErrorType.Validation);
    public static readonly Error PriceAtBookingMustBeGreaterThanZero = new("Appointment.PriceAtBookingMustBeGreaterThanZero", "Price at booking must be greater than zero", ErrorType.Validation);

    public static readonly Error InvalidStatusTransition = new("Appointment.InvalidStatusTransition", "Invalid status transition", ErrorType.Conflict);
    public static readonly Error TimeSlotUnavailable = new("Appointment.TimeSlotUnavailable", "Time slot is unavailable", ErrorType.Conflict);
}