using Appointments.Application.Dtos.Services;

namespace Appointments.Application.Interfaces.Services;

public interface IServiceService
{
    Task<IEnumerable<ServiceResponse>> GetAllAsync();
    Task<ServiceResponse?> GetByIdAsync(Guid id);
    Task<ServiceResponse> CreateAsync(CreateServiceRequest request);
    Task<ServiceResponse?> UpdateAsync(Guid id, UpdateServiceRequest request);
    Task<ServiceResponse?> DeleteAsync(Guid id);
}