using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.DeleteAppointment;

public interface IDeleteAppointmentCommandHandler
{
    Task<Result> HandleAsync(DeleteAppointmentCommand command, CancellationToken cancellationToken = default);
}
