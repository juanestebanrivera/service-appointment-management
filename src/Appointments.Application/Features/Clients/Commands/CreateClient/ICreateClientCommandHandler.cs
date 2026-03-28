using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Commands.CreateClient;

public interface ICreateClientCommandHandler
{
    Task<Result<Guid>> HandleAsync(CreateClientCommand command, CancellationToken cancellationToken = default);
}