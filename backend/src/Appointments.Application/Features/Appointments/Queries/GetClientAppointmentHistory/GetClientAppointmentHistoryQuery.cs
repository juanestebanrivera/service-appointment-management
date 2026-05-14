namespace Appointments.Application.Features.Appointments.Queries.GetClientAppointmentHistory;

public sealed record GetClientAppointmentHistoryQuery(Guid ClientId, int Page, int PageSize);
