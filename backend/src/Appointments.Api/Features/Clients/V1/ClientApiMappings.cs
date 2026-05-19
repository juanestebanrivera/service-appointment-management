using Appointments.Api.Features.Clients.V1.Contracts;
using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Clients;

namespace Appointments.Api.Features.Clients.V1;

public static class ClientApiMappings
{

    public static ClientApiResponse ToClientApiResponse(this ClientResult result)
    {
        return new ClientApiResponse(
            result.Id,
            result.FirstName,
            result.LastName,
            result.PhonePrefix,
            result.PhoneNumber,
            result.Email,
            result.IsActive
        );
    }

    public static ClientAppointmentApiResponse ToClientAppointmentApiResponse(this ClientAppointmentResult result)
    {
        return new ClientAppointmentApiResponse(
            result.Id,
            result.StartTime,
            result.EndTime,
            result.Status.ToString(),
            result.PriceAtBooking,
            new ClientAppointmentServiceResponse(result.ServiceId, result.ServiceName)
        );
    }
}