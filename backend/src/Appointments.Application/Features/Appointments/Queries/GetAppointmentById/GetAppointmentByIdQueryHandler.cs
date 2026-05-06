using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public sealed class GetAppointmentByIdQueryHandler(IAppointmentRepository appointmentRepository)
    : IQueryHandler<GetAppointmentByIdQuery, AppointmentResult>
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;

    public async Task<Result<AppointmentResult>> HandleAsync(GetAppointmentByIdQuery query, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(query.AppointmentId, cancellationToken);

        if (appointment is null)
            return Result<AppointmentResult>.Failure(AppointmentApplicationErrors.NotFound);

        return Result<AppointmentResult>.Success(appointment.ToAppointmentResult());
    }
}
