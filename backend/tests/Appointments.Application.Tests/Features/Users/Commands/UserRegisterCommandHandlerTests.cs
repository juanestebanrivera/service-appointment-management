using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Users;
using Appointments.Application.Features.Users.Commands.UserRegister;
using Appointments.Domain.Users;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Users.Commands;

public class UserRegisterCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserRegisterCommandHandler _handler;

    public UserRegisterCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UserRegisterCommandHandler(_userRepository, _passwordHasher, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenEmailIsInvalid_ReturnsFailure()
    {
        // Arrange
        var command = new UserRegisterCommand(Email: "not-an-email", Password: "password123");

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        await _userRepository.DidNotReceive().VerifyIfEmailExistsAsync(Arg.Any<string>(), Arg.Any<CancellationToken>());
        _userRepository.DidNotReceive().Add(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenEmailAlreadyExists_ReturnsFailure()
    {
        // Arrange
        var command = new UserRegisterCommand(Email: "existing@domain.com", Password: "password123");

        _userRepository.VerifyIfEmailExistsAsync(command.Email, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserApplicationErrors.InvalidEmail, result.Error);

        _userRepository.DidNotReceive().Add(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenPasswordIsEmpty_ReturnsFailure()
    {
        // Arrange
        var command = new UserRegisterCommand(Email: "username@domain.com", Password: "");

        _userRepository.VerifyIfEmailExistsAsync(command.Email, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _userRepository.DidNotReceive().Add(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenPasswordIsTooShort_ReturnsFailure()
    {
        // Arrange
        var command = new UserRegisterCommand(Email: "username@domain.com", Password: "abc");

        _userRepository.VerifyIfEmailExistsAsync(command.Email, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _userRepository.DidNotReceive().Add(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenCommandIsValid_ReturnsSuccessAndRegistersUser()
    {
        // Arrange
        User? registeredUser = null;
        var command = new UserRegisterCommand(Email: "username@domain.com", Password: "password123");
        var hashedPassword = "hashedpassword";

        _userRepository.VerifyIfEmailExistsAsync(command.Email, Arg.Any<CancellationToken>()).Returns(false);
        _passwordHasher.Hash(command.Password).Returns(hashedPassword);
        _userRepository.Add(Arg.Do<User>(u => registeredUser = u));

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(registeredUser);
        Assert.NotEqual(Guid.Empty, registeredUser.Id);
        Assert.Equal(command.Email, registeredUser.Email.Value);
        Assert.True(registeredUser.IsActive);
        Assert.Equal(hashedPassword, registeredUser.PasswordHash);

        _userRepository.Received(1).Add(Arg.Any<User>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
