using Appointments.Application.Common.Interfaces;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentsByDate;

public sealed class GetAppointmentsByDateQueryHandler(IAppointmentQueryRepository appointmentRepository)
    : IQueryHandler<GetAppointmentsByDateQuery, IEnumerable<AppointmentDetailResult>>
{
    private readonly IAppointmentQueryRepository _appointmentRepository = appointmentRepository;

    public async Task<Result<IEnumerable<AppointmentDetailResult>>> HandleAsync(GetAppointmentsByDateQuery query, CancellationToken cancellationToken = default)
    {
        if (query.Date == default)
            return Result<IEnumerable<AppointmentDetailResult>>.Failure(AppointmentApplicationErrors.DateIsRequired);

        var appointments = await _appointmentRepository.GetByDateAsync(query.Date, cancellationToken);

        return Result<IEnumerable<AppointmentDetailResult>>.Success(appointments);
    }
}
