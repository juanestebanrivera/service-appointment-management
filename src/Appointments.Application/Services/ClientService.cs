using Appointments.Application.Dtos.Clients;
using Appointments.Application.Interfaces.Repositories;
using Appointments.Application.Interfaces.Services;
using Appointments.Application.Mappings;
using Appointments.Domain.Entities;

namespace Appointments.Application.Services;

public class ClientService(IClientRepository repository) : IClientService
{
    private readonly IClientRepository _repository = repository;

    public async Task<IEnumerable<ClientResponse>> GetAllAsync()
    {
        var clients = await _repository.GetAllAsync();
        return clients.Select(client => client.ToClientResponse());
    }

    public async Task<ClientResponse?> GetByIdAsync(Guid id)
    {
        var client = await _repository.GetByIdAsync(id);

        return client?.ToClientResponse();
    }

    public async Task<ClientResponse> CreateAsync(CreateClientRequest request)
    {
        var client = new Client(request.Name, request.Email, request.DateOfBith);

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            client.UpdatePhoneNumber(client.PhoneNumber);
        }

        await _repository.AddAsync(client);

        return client.ToClientResponse();
    }

    public async Task<ClientResponse?> UpdateContactInformationAsync(Guid id, UpdateContactInformationClientRequest request)
    {
        var client = await _repository.GetByIdAsync(id);

        if (client is null)
            return null;

        if (!string.IsNullOrWhiteSpace(request.Email))
        {
            client.UpdateEmail(request.Email);
        }

        if (!string.IsNullOrWhiteSpace(request.PhoneNumber))
        {
            client.UpdatePhoneNumber(request.PhoneNumber);
        }

        await _repository.UpdateAsync(client);
        return client.ToClientResponse();
    }

    public async Task<ClientResponse?> DeleteAsync(Guid id)
    {
        var client = await _repository.GetByIdAsync(id);

        if (client is null)
            return null;

        await _repository.DeleteAsync(id);
        return client.ToClientResponse();
    }
}