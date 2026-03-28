using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Commands.UpdateService;

public interface IUpdateServiceCommandHandler
{
    Task<Result> HandleAsync(UpdateServiceCommand command, CancellationToken cancellationToken = default);
}