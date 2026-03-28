using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Commands.CreateService;

public interface ICreateServiceCommandHandler
{
    Task<Result<Guid>> HandleAsync(CreateServiceCommand command, CancellationToken cancellationToken = default);
}
