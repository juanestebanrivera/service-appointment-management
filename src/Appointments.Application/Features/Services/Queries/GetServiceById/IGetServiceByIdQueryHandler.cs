using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Services.Queries.GetServiceById;

public interface IGetServiceByIdQueryHandler
{
    Task<Result<ServiceResponse>> HandleAsync(GetServiceByIdQuery query, CancellationToken cancellationToken = default);
}
