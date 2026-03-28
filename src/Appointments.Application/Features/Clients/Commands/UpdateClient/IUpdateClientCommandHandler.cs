using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Commands.UpdateClient;

interface IUpdateClientCommandHandler
{
    Task<Result> HandleAsync(UpdateClientCommand command, CancellationToken cancellationToken = default);
}
