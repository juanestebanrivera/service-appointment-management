using Appointments.Application.Common.Interfaces;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public sealed class GetAppointmentByIdQueryHandler(
    IAppointmentQueryRepository appointmentRepository
) : IQueryHandler<GetAppointmentByIdQuery, AppointmentDetailResult>
{
    private readonly IAppointmentQueryRepository _appointmentRepository = appointmentRepository;

    public async Task<Result<AppointmentDetailResult>> HandleAsync(GetAppointmentByIdQuery query, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetDetailByIdAsync(query.AppointmentId, cancellationToken);

        if (appointment is null)
            return Result<AppointmentDetailResult>.Failure(AppointmentApplicationErrors.NotFound);

        if (!query.IsAdmin && appointment.ClientUserId != query.CurrentUserId)
            return Result<AppointmentDetailResult>.Failure(AppointmentApplicationErrors.Forbidden);

        return Result<AppointmentDetailResult>.Success(appointment);
    }
}
