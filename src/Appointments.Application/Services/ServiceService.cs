using Appointments.Application.Dtos.Services;
using Appointments.Application.Interfaces.Repositories;
using Appointments.Application.Interfaces.Services;
using Appointments.Application.Mappings;
using Appointments.Domain.Entities;

namespace Appointments.Application.Services;

public class ServiceService(IServiceRepository repository) : IServiceService
{
    private readonly IServiceRepository _repository = repository;

    public async Task<IEnumerable<ServiceResponse>> GetAllAsync()
    {
        var services = await _repository.GetAllAsync();
        return services.Select(service => service.ToServiceResponse());
    }

    public async Task<ServiceResponse?> GetByIdAsync(Guid id)
    {
        var service = await _repository.GetByIdAsync(id);
        return service?.ToServiceResponse();
    }

    public async Task<ServiceResponse> CreateAsync(CreateServiceRequest request)
    {
        var service = new Service(request.Name, request.Price, request.EstimatedDuration);

        await _repository.AddAsync(service);

        return service.ToServiceResponse();
    }

    public async Task<ServiceResponse?> UpdateAsync(Guid id, UpdateServiceRequest request)
    {
        var service = await _repository.GetByIdAsync(id);

        if (service is null)
            return null;
        
        service.Name = request.Name;
        service.Price = request.Price;
        service.EstimatedDuration = request.EstimatedDuration;
        service.IsActive = request.IsActive;

        await _repository.UpdateAsync(service);
        
        return service.ToServiceResponse();
    }

    public async Task<ServiceResponse?> DeleteAsync(Guid id)
    {
        var service = await _repository.GetByIdAsync(id);

        if (service is null)
            return null;

        await _repository.DeleteAsync(service);

        return service.ToServiceResponse();
    }
}