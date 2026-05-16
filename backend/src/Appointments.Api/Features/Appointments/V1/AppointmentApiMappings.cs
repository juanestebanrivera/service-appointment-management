using Appointments.Api.Features.Appointments.V1.Contracts;
using Appointments.Application.Features.Appointments;

namespace Appointments.Api.Features.Appointments.V1;

public static class AppointmentApiMappings
{
    extension(AppointmentDetailResult appointment)
    {
        public AppointmentApiResponse ToAppointmentApiResponse()
        {
            return new AppointmentApiResponse(
                appointment.Id,
                appointment.StartTime,
                appointment.EndTime,
                appointment.Status.ToString(),
                appointment.PriceAtBooking,
                new AppointmentClientResponse(
                    appointment.ClientId,
                    appointment.ClientFirstName,
                    appointment.ClientLastName,
                    appointment.ClientEmail,
                    appointment.ClientPhoneNumber,
                    appointment.ClientPhonePrefix
                ),
                new AppointmentServiceResponse(
                    appointment.ServiceId,
                    appointment.ServiceName
                )
            );
        }
    }
}