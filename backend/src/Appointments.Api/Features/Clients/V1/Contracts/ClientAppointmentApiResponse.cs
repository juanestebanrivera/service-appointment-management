namespace Appointments.Api.Features.Clients.V1.Contracts;

public record ClientAppointmentApiResponse(
    Guid Id,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    string Status,
    decimal Price,
    ClientAppointmentServiceResponse Service
);

public record ClientAppointmentServiceResponse(Guid ServiceId, string ServiceName);