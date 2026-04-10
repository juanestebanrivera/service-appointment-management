namespace Appointments.Application.Features.Services.Queries.GetAllServices;

public interface IGetAllServicesQueryHandler
{
    Task<IEnumerable<ServiceResponse>> HandleAsync(CancellationToken cancellationToken = default);
}
