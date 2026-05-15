using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Clients;
using Appointments.Application.Features.Clients.Commands.CreateClient;
using Appointments.Application.Features.Users;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel.ValueObjects;
using Appointments.Domain.Users;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Clients.Commands;

public class CreateClientCommandHandlerTests
{
    private readonly IClientRepository _clientRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateClientCommandHandler _handler;

    public CreateClientCommandHandlerTests()
    {
        _clientRepository = Substitute.For<IClientRepository>();
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateClientCommandHandler(_clientRepository, _userRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenCurrentUserIsNotTheLinkedUser_ReturnsForbidden()
    {
        // Arrange
        var command = new CreateClientCommand(
            UserId: Guid.NewGuid(),
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            CurrentUserId: Guid.NewGuid(),
            Email: "username@domain.com"
        );

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ClientApplicationErrors.Forbidden, result.Error);

        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenPhoneIsInvalid_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateClientCommand(
            UserId: userId,
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "InvalidPrefix",
            PhoneNumber: "InvalidNumber",
            CurrentUserId: userId,
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
        var userId = Guid.NewGuid();
        var command = new CreateClientCommand(
            UserId: userId,
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            CurrentUserId: userId,
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
        var userId = Guid.NewGuid();
        var command = new CreateClientCommand(
            UserId: userId,
            FirstName: "",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            CurrentUserId: userId,
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
        var userId = Guid.NewGuid();
        var command = new CreateClientCommand(
            UserId: userId,
            FirstName: "FirstName",
            LastName: "",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            CurrentUserId: userId,
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
    public async Task HandleAsync_WhenUserDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new CreateClientCommand(
            UserId: userId,
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            CurrentUserId: userId,
            Email: "username@domain.com"
        );

        _userRepository.GetByIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns((User?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserApplicationErrors.UserNotFound, result.Error);

        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenUserIsInactive_ReturnsFailure()
    {
        // Arrange
        var user = CreateActiveUser();
        user.Deactivate();

        var command = new CreateClientCommand(
            UserId: user.Id,
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            CurrentUserId: user.Id,
            Email: "username@domain.com"
        );

        _userRepository.GetByIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(user);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserApplicationErrors.UserNotFound, result.Error);

        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenPhoneIsAlreadyInUse_ReturnsFailure()
    {
        // Arrange
        var user = CreateActiveUser();
        var command = new CreateClientCommand(
            UserId: user.Id,
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            CurrentUserId: user.Id,
            Email: "username@domain.com"
        );

        _clientRepository.ExistsByPhoneAsync(Arg.Any<PhoneNumber>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ClientApplicationErrors.PhoneAlreadyInUse, result.Error);

        await _userRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenEmailIsAlreadyInUse_ReturnsFailure()
    {
        // Arrange
        var user = CreateActiveUser();
        var command = new CreateClientCommand(
            UserId: user.Id,
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            CurrentUserId: user.Id,
            Email: "username@domain.com"
        );

        _clientRepository.ExistsByPhoneAsync(Arg.Any<PhoneNumber>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);
        _clientRepository.ExistsByEmailAsync(Arg.Any<Email>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ClientApplicationErrors.EmailAlreadyInUse, result.Error);

        await _userRepository.DidNotReceive().GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        _clientRepository.DidNotReceive().Add(Arg.Any<Client>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(null)]
    [InlineData("username@domain.com")]
    public async Task HandleAsync_WhenCommandIsValid_ReturnsSuccessAndCreatesClient(string? email)
    {
        // Arrange
        Client? createdClient = null;
        var user = CreateActiveUser();
        var command = new CreateClientCommand(
            UserId: user.Id,
            FirstName: "FirstName",
            LastName: "LastName",
            PhonePrefix: "+1",
            PhoneNumber: "1234567890",
            CurrentUserId: user.Id,
            Email: email
        );

        _clientRepository.Add(Arg.Do<Client>(c => createdClient = c));
        _userRepository.GetByIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(user);
        _clientRepository.ExistsByPhoneAsync(Arg.Any<PhoneNumber>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);
        _clientRepository.ExistsByEmailAsync(Arg.Any<Email>(), Arg.Any<Guid?>(), Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(createdClient);
        Assert.NotEqual(Guid.Empty, createdClient.Id);
        Assert.Equal(createdClient.Id, result.Value);
        Assert.Equal(command.FirstName, createdClient.FirstName.Value);
        Assert.Equal(command.LastName, createdClient.LastName.Value);
        Assert.Equal(command.PhonePrefix, createdClient.Phone.Prefix);
        Assert.Equal(command.PhoneNumber, createdClient.Phone.Number);
        Assert.Equal(command.Email, createdClient.Email?.Value);
        Assert.True(createdClient.IsActive);

        _clientRepository.Received(1).Add(Arg.Any<Client>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private static User CreateActiveUser()
    {
        var passwordHasher = Substitute.For<IPasswordHasher>();
        passwordHasher.Hash(Arg.Any<string>()).Returns("HashedPassword");

        var user = User.Register(Email.Create("username@domain.com").Value, "UserPassword", passwordHasher).Value;

        return user;
    }
}