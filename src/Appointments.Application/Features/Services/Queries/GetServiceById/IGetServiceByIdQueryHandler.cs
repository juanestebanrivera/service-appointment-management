using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Queries.GetServiceById;

public interface IGetServiceByIdQueryHandler
{
    Task<Result<ServiceResponse>> HandleAsync(Guid id, CancellationToken cancellationToken = default);
}
