using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public sealed class GetAllAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
    : IQueryHandler<GetAllAppointmentsQuery, IEnumerable<AppointmentResponse>>
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;

    public async Task<Result<IEnumerable<AppointmentResponse>>> HandleAsync(GetAllAppointmentsQuery query, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<AppointmentResponse>>.Success(appointments.Select(appointment => appointment.ToAppointmentResponse()));
    }
}
