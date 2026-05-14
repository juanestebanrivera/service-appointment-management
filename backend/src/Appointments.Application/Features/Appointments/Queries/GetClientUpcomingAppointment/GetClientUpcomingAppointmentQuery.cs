namespace Appointments.Application.Features.Appointments.Queries.GetClientUpcomingAppointment;

public sealed record GetClientUpcomingAppointmentQuery(Guid ClientId, bool IncludeLast);
