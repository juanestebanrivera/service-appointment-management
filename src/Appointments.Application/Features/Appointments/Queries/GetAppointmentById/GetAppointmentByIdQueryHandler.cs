using Appointments.Application.Common.Interfaces;
using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public sealed class GetAppointmentByIdQueryHandler(IAppointmentRepository appointmentRepository)
    : IQueryHandler<GetAppointmentByIdQuery, AppointmentResponse>
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;

    public async Task<Result<AppointmentResponse>> HandleAsync(GetAppointmentByIdQuery query, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(query.AppointmentId, cancellationToken);

        if (appointment is null)
            return Result<AppointmentResponse>.Failure(AppointmentApplicationErrors.NotFound);

        return Result<AppointmentResponse>.Success(appointment.ToAppointmentResponse());
    }
}
