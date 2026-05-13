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
                    FullName: $"{appointment.ClientFirstName} {appointment.ClientLastName}",
                    Email: appointment.ClientEmail,
                    Phone: $"{appointment.ClientPhonePrefix}{appointment.ClientPhoneNumber}"
                ),
                new AppointmentServiceResponse(
                    appointment.ServiceId,
                    appointment.ServiceName
                )
            );
        }
    }
}