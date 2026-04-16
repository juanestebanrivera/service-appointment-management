using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Clients.Commands.UpdateClient;
using Appointments.Domain.Clients;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Clients.Commands;

public class UpdateClientCommandHandlerTests
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateClientCommandHandler _handler;

    public UpdateClientCommandHandlerTests()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateClientCommandHandler(_clientRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenClientDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new UpdateClientCommand(
            ClientId: Guid.NewGuid(),
            FirstName: "FirstName",
            LastName: "LastName",
            Email: "username@domain.com",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            IsActive: true
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns((Client?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _clientRepository.DidNotReceive().Update(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenFirstNameIsInvalid_ReturnsFailure()
    {
        // Arrange
        var client = CreateValidClient();
        var command = new UpdateClientCommand(
            ClientId: client.Id,
            FirstName: "",
            LastName: "LastName",
            Email: "username@domain.com",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            IsActive: true
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _clientRepository.DidNotReceive().Update(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenLastNameIsInvalid_ReturnsFailure()
    {
        // Arrange
        var client = CreateValidClient();
        var command = new UpdateClientCommand(
            ClientId: client.Id,
            FirstName: "FirstName",
            LastName: "",
            Email: "username@domain.com",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            IsActive: true
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _clientRepository.DidNotReceive().Update(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenEmailIsInvalid_ReturnsFailure()
    {
        // Arrange
        var client = CreateValidClient();
        var command = new UpdateClientCommand(
            ClientId: client.Id,
            FirstName: "FirstName",
            LastName: "LastName",
            Email: "invalid-email",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            IsActive: true
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _clientRepository.DidNotReceive().Update(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenPhoneIsInvalid_ReturnsFailure()
    {
        // Arrange
        var client = CreateValidClient();
        var command = new UpdateClientCommand(
            ClientId: client.Id,
            FirstName: "FirstName",
            LastName: "LastName",
            Email: "username@domain.com",
            PhonePrefix: "+1",
            PhoneNumber: "invalid-phone",
            IsActive: true
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _clientRepository.DidNotReceive().Update(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("newusername@domain.com", true)]
    [InlineData("username@domain.com", true)]
    [InlineData(null, false)]
    [InlineData("newusername@domain.com", false)]
    [InlineData("username@domain.com", false)]
    public async Task HandleAsync_WhenClientExistsAndDataIsValid_ReturnsSuccessAndUpdatesClient(string? email, bool isActive)
    {
        // Arrange
        Client? updatedClient = null;
        var client = CreateValidClient();

        var command = new UpdateClientCommand(
            ClientId: client.Id,
            FirstName: "NewFirstName",
            LastName: "NewLastName",
            Email: email,
            PhonePrefix: "+57",
            PhoneNumber: "0987654321",
            IsActive: isActive
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);
        _clientRepository.Update(Arg.Do<Client>(c => updatedClient = c));

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(updatedClient);
        Assert.Equal(command.ClientId, updatedClient.Id);
        Assert.Equal(command.FirstName, updatedClient.FirstName.Value);
        Assert.Equal(command.LastName, updatedClient.LastName.Value);
        Assert.Equal(command.Email, updatedClient.Email?.Value);
        Assert.Equal(command.PhonePrefix, updatedClient.Phone.Prefix);
        Assert.Equal(command.PhoneNumber, updatedClient.Phone.Number);
        Assert.Equal(command.IsActive, updatedClient.IsActive);

        _clientRepository.Received(1).Update(Arg.Any<Client>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private static Client CreateValidClient()
    {
        return Client.Register(
            PersonName.Create("FirstName", nameof(Client.FirstName)).Value,
            PersonName.Create("LastName", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+1", "1234567890").Value,
            Email.Create("username@domain.com").Value
        ).Value;
    }
}
