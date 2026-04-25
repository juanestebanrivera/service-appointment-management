using Appointments.Application.Features.Clients;
using Appointments.Application.Features.Clients.Queries.GetClientById;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel.ValueObjects;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Clients.Queries;

public class GetClientByIdQueryHandlerTests
{
    private readonly IClientRepository _clientRepository;
    private readonly GetClientByIdQueryHandler _handler;

    public GetClientByIdQueryHandlerTests()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _handler = new GetClientByIdQueryHandler(_clientRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenClientDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var query = new GetClientByIdQuery(Guid.NewGuid());
        _clientRepository.GetByIdAsync(query.ClientId, Arg.Any<CancellationToken>()).Returns((Client?)null);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ClientApplicationErrors.NotFound, result.Error);
    }

    [Fact]
    public async Task HandleAsync_WhenClientExists_ReturnsSuccessWithClientData()
    {
        // Arrange
        var client = Client.Register(
            PersonName.Create("FirstName", nameof(Client.FirstName)).Value,
            PersonName.Create("LastName", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+1", "1234567890").Value,
            Email.Create("username@domain.com").Value
        ).Value;

        var query = new GetClientByIdQuery(client.Id);
        _clientRepository.GetByIdAsync(query.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(client.Id, result.Value.Id);
        Assert.Equal(client.FirstName.Value, result.Value.FirstName);
        Assert.Equal(client.LastName.Value, result.Value.LastName);
        Assert.Equal(client.Phone.Prefix, result.Value.PhonePrefix);
        Assert.Equal(client.Phone.Number, result.Value.PhoneNumber);
        Assert.Equal(client.Email?.Value, result.Value.Email);
        Assert.Equal(client.IsActive, result.Value.IsActive);
    }
}