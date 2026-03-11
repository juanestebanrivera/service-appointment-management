using Appointments.Application.Dtos.Appointments;
using Appointments.Domain.Entities;

namespace Appointments.Application.Mappings;

public static class AppointmentExtension
{
    extension(Appointment appointment)
    {
        public AppointmentResponse ToAppointmentResponse()
        {
            return new AppointmentResponse(
                appointment.Id,
                appointment.ClientId,
                appointment.ServiceId,
                appointment.ScheduledTime,
                appointment.Duration,
                appointment.Status
            );
        }
    }
}