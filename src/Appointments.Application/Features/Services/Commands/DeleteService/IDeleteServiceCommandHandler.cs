using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Commands.DeleteService;

public interface IDeleteServiceCommandHandler
{
    Task<Result> HandleAsync(DeleteServiceCommand command, CancellationToken cancellationToken = default);
}
