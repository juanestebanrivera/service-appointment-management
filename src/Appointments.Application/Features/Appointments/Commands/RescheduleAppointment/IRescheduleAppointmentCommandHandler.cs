using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;

public interface IRescheduleAppointmentCommandHandler
{
    Task<Result> HandleAsync(RescheduleAppointmentCommand command, CancellationToken cancellationToken = default);
}
