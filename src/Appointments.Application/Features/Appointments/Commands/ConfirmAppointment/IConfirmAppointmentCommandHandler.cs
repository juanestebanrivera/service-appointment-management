using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.ConfirmAppointment;

public interface IConfirmAppointmentCommandHandler
{
    Task<Result> HandleAsync(ConfirmAppointmentCommand command, CancellationToken cancellationToken = default);
}
