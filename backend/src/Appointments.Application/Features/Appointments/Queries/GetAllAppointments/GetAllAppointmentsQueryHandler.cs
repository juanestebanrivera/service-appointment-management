using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public sealed class GetAllAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
    : IQueryHandler<GetAllAppointmentsQuery, IEnumerable<AppointmentResult>>
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;

    public async Task<Result<IEnumerable<AppointmentResult>>> HandleAsync(GetAllAppointmentsQuery query, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAsync(cancellationToken);

        return Result<IEnumerable<AppointmentResult>>.Success(appointments.Select(appointment => appointment.ToAppointmentResult()));
    }
}
