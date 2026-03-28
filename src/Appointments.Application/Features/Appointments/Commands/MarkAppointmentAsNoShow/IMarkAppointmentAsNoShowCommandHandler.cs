using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.MarkAppointmentAsNoShow;

public interface IMarkAppointmentAsNoShowCommandHandler
{
    Task<Result> HandleAsync(MarkAppointmentAsNoShowCommand command, CancellationToken cancellationToken = default);
}
