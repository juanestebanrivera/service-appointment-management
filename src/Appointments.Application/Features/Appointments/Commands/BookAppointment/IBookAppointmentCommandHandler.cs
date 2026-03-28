using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Appointments.Commands.BookAppointment;

public interface IBookAppointmentCommandHandler
{
    Task<Result<Guid>> HandleAsync(BookAppointmentCommand command, CancellationToken cancellationToken = default);
}
