using Appointments.Domain.SharedKernel;

namespace Appointments.Domain.Appointments;

public static class AppointmentErrors
{
    public static readonly Error ClientIsRequired = new("Appointment.ClientIsRequired", "Client is required");

    public static readonly Error ServiceIsRequired = new("Appointment.ServiceIsRequired", "Service is required");
    public static readonly Error PriceAtBookingMustBeGreaterThanZero = new("Appointment.PriceAtBookingMustBeGreaterThanZero", "Price at booking must be greater than zero.");

    public static readonly Error InvalidStatusTransition = new("Appointment.InvalidStatusTransition", "Invalid status transition.");
}