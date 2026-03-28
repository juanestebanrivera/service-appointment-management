using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public interface IGetAppointmentByIdQueryHandler
{
    Task<Result<AppointmentResponse>> HandleAsync(GetAppointmentByIdQuery query, CancellationToken cancellationToken = default);
}
