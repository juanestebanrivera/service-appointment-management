using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public interface IGetAppointmentByIdQueryHandler
{
    Task<Result<AppointmentResponse>> HandleAsync(Guid id, CancellationToken cancellationToken = default);
}
