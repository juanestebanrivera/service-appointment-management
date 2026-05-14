namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public record GetAppointmentByIdQuery(
    Guid AppointmentId,
    Guid CurrentUserId,
    bool IsAdmin
);