using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Commands.DeleteClient;

public interface IDeleteClientCommandHandler
{
    Task<Result> HandleAsync(DeleteClientCommand command, CancellationToken cancellationToken = default);
}
