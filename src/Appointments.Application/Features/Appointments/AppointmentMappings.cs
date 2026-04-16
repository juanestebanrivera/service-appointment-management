using Appointments.Domain.Appointments;

namespace Appointments.Application.Features.Appointments;

public static class AppointmentMappings
{
    extension(Appointment appointment)
    {
        public AppointmentResult ToAppointmentResult()
        {
            return new AppointmentResult(
                appointment.Id,
                appointment.ClientId,
                appointment.ServiceId,
                appointment.PriceAtBooking,
                appointment.TimeRange.StartTime,
                appointment.TimeRange.EndTime,
                appointment.Status
            );
        }
    }
}