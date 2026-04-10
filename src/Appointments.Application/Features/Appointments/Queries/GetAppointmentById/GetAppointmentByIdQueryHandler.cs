using Appointments.Domain.Appointments;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public class GetAppointmentByIdQueryHandler(IAppointmentRepository appointmentRepository) : IGetAppointmentByIdQueryHandler
{
    private readonly IAppointmentRepository _appointmentRepository = appointmentRepository;

    public async Task<Result<AppointmentResponse>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);

        if (appointment is null)
            return Result<AppointmentResponse>.Failure(AppointmentApplicationErrors.NotFound);

        return Result<AppointmentResponse>.Success(appointment.ToAppointmentResponse());
    }
}
