using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.CancelAppointment;

public interface ICancelAppointmentCommandHandler
{
    Task<Result> HandleAsync(CancelAppointmentCommand command, CancellationToken cancellationToken = default);
}
