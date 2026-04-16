using Appointments.Api.Features.Appointments.V1.Contracts;
using Appointments.Application.Features.Appointments;

namespace Appointments.Api.Features.Appointments.V1;

public static class AppointmentApiMappings
{
    extension(AppointmentResult appointment)
    {
        public AppointmentApiResponse ToAppointmentApiResponse()
        {
            return new AppointmentApiResponse(
                appointment.Id,
                appointment.ClientId,
                appointment.ServiceId,
                appointment.PriceAtBooking,
                appointment.StartTime,
                appointment.EndTime,
                appointment.Status
            );
        }
    }
}