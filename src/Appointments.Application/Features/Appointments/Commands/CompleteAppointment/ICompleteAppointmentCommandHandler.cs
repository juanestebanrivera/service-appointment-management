using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.CompleteAppointment;

public interface ICompleteAppointmentCommandHandler
{
    Task<Result> HandleAsync(CompleteAppointmentCommand command, CancellationToken cancellationToken = default);
}
