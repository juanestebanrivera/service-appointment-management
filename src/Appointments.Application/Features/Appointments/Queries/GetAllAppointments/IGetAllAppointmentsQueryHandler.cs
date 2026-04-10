namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public interface IGetAllAppointmentsQueryHandler
{
    Task<IEnumerable<AppointmentResponse>> HandleAsync(CancellationToken cancellationToken = default);
}
