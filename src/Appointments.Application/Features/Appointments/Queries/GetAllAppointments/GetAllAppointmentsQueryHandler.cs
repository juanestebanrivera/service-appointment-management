using Appointments.Domain.Appointments;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public sealed class GetAllAppointmentsQueryHandler(IAppointmentRepository appointmentRepository) : IGetAllAppointmentsQueryHandler
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;

    public async Task<IEnumerable<AppointmentResponse>> HandleAsync(GetAllAppointmentsQuery query, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);

        return appointments.Select(appointment => appointment.ToAppointmentResponse());
    }
}
