using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Clients.Commands.CreateClient;
using Appointments.Domain.Clients;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Clients.Commands;

public class CreateClientCommandHandlerTests
{
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateClientCommandHandler _handler;

    public CreateClientCommandHandlerTests()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateClientCommandHandler(_clientRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenPhoneIsInvalid_ReturnsFailure()
    {
        // Arrange
        var command = new CreateClientCommand(
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "InvalidPrefix",
            PhoneNumber: "InvalidNumber",
            Email: "username@domain.com"
        );

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenEmailIsInvalid_ReturnsFailure()
    {
        // Arrange
        var command = new CreateClientCommand(
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            Email: "InvalidEmail"
        );

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenFirstNameIsInvalid_ReturnsFailure()
    {
        // Arrange
        var command = new CreateClientCommand(
            FirstName: "",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            Email: "username@domain.com"
        );

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenLastNameIsInvalid_ReturnsFailure()
    {
        // Arrange
        var command = new CreateClientCommand(
            FirstName: "FirstName",
            LastName: "",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            Email: "username@domain.com"
        );

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("username@domain.com")]
    public async Task HandleAsync_WhenCommandIsValid_ReturnsSuccessAndCreatesClient(string? email)
    {
        // Arrange
        Client? capturedClient = null;
        var command = new CreateClientCommand(
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            Email: email
        );

        _clientRepository.Add(Arg.Do<Client>(c => capturedClient = c));

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(capturedClient);
        Assert.NotEqual(Guid.Empty, capturedClient.Id);
        Assert.Equal(command.FirstName, capturedClient.FirstName.Value);
        Assert.Equal(command.LastName, capturedClient.LastName.Value);
        Assert.Equal(command.PhonePrefix, capturedClient.Phone.Prefix);
        Assert.Equal(command.PhoneNumber, capturedClient.Phone.Number);
        Assert.Equal(email, capturedClient.Email?.Value);
        Assert.True(capturedClient.IsActive);

        _clientRepository.Received(1).Add(Arg.Any<Client>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}