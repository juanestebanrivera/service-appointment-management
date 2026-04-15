using Appointments.Application.Features.Clients.Queries.GetAllClients;
using Appointments.Domain.Clients;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Clients.Queries;

public class GetAllClientsQueryHandlerTests
{
    private readonly IClientRepository _clientRepository;
    private readonly GetAllClientsQueryHandler _handler;

    public GetAllClientsQueryHandlerTests()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _handler = new GetAllClientsQueryHandler(_clientRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenClientsExist_ReturnsSuccessWithClientData()
    {
        // Arrange
        var query = new GetAllClientsQuery();
        var clients = new List<Client>
        {
            Client.Register(
                PersonName.Create("FirstNameOne", nameof(Client.FirstName)).Value,
                PersonName.Create("LastNameOne", nameof(Client.LastName)).Value,
                PhoneNumber.Create("+1", "1234567890").Value,
                Email.Create("username1@domain.com").Value
            ).Value,
            Client.Register(
                PersonName.Create("FirstNameTwo", nameof(Client.FirstName)).Value,
                PersonName.Create("LastNameTwo", nameof(Client.LastName)).Value,
                PhoneNumber.Create("+1", "0987654321").Value,
                Email.Create("username2@domain.com").Value
            ).Value
        };

        _clientRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(clients);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(clients.Count, result.Value.Count());

        var firstClientResponse = result.Value.First();
        Assert.Equal(clients[0].Id, firstClientResponse.Id);
        Assert.Equal(clients[0].FirstName.Value, firstClientResponse.FirstName);
        Assert.Equal(clients[0].LastName.Value, firstClientResponse.LastName);
        Assert.Equal(clients[0].Phone.Prefix, firstClientResponse.PhonePrefix);
        Assert.Equal(clients[0].Phone.Number, firstClientResponse.PhoneNumber);
        Assert.Equal(clients[0].Email?.Value, firstClientResponse.Email);
        Assert.Equal(clients[0].IsActive, firstClientResponse.IsActive);
    }

    [Fact]
    public async Task HandleAsync_WhenNoClientsExist_ReturnsSuccessWithEmptyList()
    {
        // Arrange
        var query = new GetAllClientsQuery();
        _clientRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }
}