using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Users;
using Appointments.Application.Features.Users.Commands.UserLogin;
using Appointments.Domain.SharedKernel.ValueObjects;
using Appointments.Domain.Users;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Users.Commands;

public class UserLoginCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly UserLoginCommandHandler _handler;

    public UserLoginCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _tokenGenerator = Substitute.For<ITokenGenerator>();
        _handler = new UserLoginCommandHandler(_userRepository, _passwordHasher, _tokenGenerator);
    }

    [Fact]
    public async Task HandleAsync_WhenUserDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new UserLoginCommand(Email: "notfound@domain.com", Password: "password123");

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns((User?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserApplicationErrors.InvalidCredentials, result.Error);

        _passwordHasher.DidNotReceive().Verify(Arg.Any<string>(), Arg.Any<string>());
        _tokenGenerator.DidNotReceive().GenerateToken(Arg.Any<User>());
    }

    [Fact]
    public async Task HandleAsync_WhenUserIsInactive_ReturnsFailure()
    {
        // Arrange
        var user = CreateActiveUser();
        user.Deactivate();

        var command = new UserLoginCommand(Email: user.Email.Value, Password: "password123");

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserApplicationErrors.InvalidCredentials, result.Error);

        _passwordHasher.DidNotReceive().Verify(Arg.Any<string>(), Arg.Any<string>());
        _tokenGenerator.DidNotReceive().GenerateToken(Arg.Any<User>());
    }

    [Fact]
    public async Task HandleAsync_WhenPasswordIsIncorrect_ReturnsFailure()
    {
        // Arrange
        var user = CreateActiveUser();
        var command = new UserLoginCommand(Email: user.Email.Value, Password: "wrongpassword");

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.Verify(command.Password, user.PasswordHash).Returns(false);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserApplicationErrors.InvalidCredentials, result.Error);

        _tokenGenerator.DidNotReceive().GenerateToken(Arg.Any<User>());
    }

    [Fact]
    public async Task HandleAsync_WhenCredentialsAreValid_ReturnsSuccessWithToken()
    {
        // Arrange
        const string expectedToken = "generated.jwt.token";
        var user = CreateActiveUser();
        var command = new UserLoginCommand(Email: user.Email.Value, Password: "password123");

        _userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>()).Returns(user);
        _passwordHasher.Verify(command.Password, user.PasswordHash).Returns(true);
        _tokenGenerator.GenerateToken(user).Returns(expectedToken);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(expectedToken, result.Value.Token);

        _tokenGenerator.Received(1).GenerateToken(user);
    }

    private User CreateActiveUser()
    {
        _passwordHasher.Hash(Arg.Any<string>()).Returns("hashedpassword");

        return User.Register(
            Email.Create("username@domain.com").Value,
            password: "password123",
            _passwordHasher
        ).Value;
    }
}
