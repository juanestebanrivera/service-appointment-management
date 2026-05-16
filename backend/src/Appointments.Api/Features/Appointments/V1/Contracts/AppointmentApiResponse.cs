namespace Appointments.Api.Features.Appointments.V1.Contracts;

public record AppointmentClientResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string? Email,
    string Phone,
    string PhonePrefix
);

public record AppointmentServiceResponse(
    Guid Id,
    string Name
);

public record AppointmentApiResponse(
    Guid Id,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    string Status,
    decimal Price,
    AppointmentClientResponse Client,
    AppointmentServiceResponse Service
);