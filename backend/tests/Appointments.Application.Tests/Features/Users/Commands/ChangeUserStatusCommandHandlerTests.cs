using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Users;
using Appointments.Application.Features.Users.Commands.ChangeUserStatus;
using Appointments.Domain.SharedKernel.ValueObjects;
using Appointments.Domain.Users;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Users.Commands;

public class ChangeUserStatusCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ChangeUserStatusCommandHandler _handler;

    public ChangeUserStatusCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new ChangeUserStatusCommandHandler(_userRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenUserDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new ChangeUserStatusCommand(UserId: Guid.NewGuid(), IsActive: true);

        _userRepository.GetByIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns((User?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserApplicationErrors.UserNotFound, result.Error);

        _userRepository.DidNotReceive().Update(Arg.Any<User>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenIsActiveIsTrue_ActivatesUserAndReturnsSuccess()
    {
        // Arrange
        User? updatedUser = null;
        var user = CreateActiveUser();
        user.Deactivate();

        var command = new ChangeUserStatusCommand(UserId: user.Id, IsActive: true);

        _userRepository.GetByIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(user);
        _userRepository.Update(Arg.Do<User>(u => updatedUser = u));

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(updatedUser);
        Assert.True(updatedUser.IsActive);

        _userRepository.Received(1).Update(user);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenIsActiveIsFalse_DeactivatesUserAndReturnsSuccess()
    {
        // Arrange
        User? updatedUser = null;
        var user = CreateActiveUser();
        var command = new ChangeUserStatusCommand(UserId: user.Id, IsActive: false);

        _userRepository.GetByIdAsync(command.UserId, Arg.Any<CancellationToken>()).Returns(user);
        _userRepository.Update(Arg.Do<User>(u => updatedUser = u));

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(updatedUser);
        Assert.False(updatedUser.IsActive);

        _userRepository.Received(1).Update(user);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private static User CreateActiveUser()
    {
        var passwordHasher = Substitute.For<IPasswordHasher>();
        passwordHasher.Hash(Arg.Any<string>()).Returns("hashedpassword");

        return User.Register(
            Email.Create("username@domain.com").Value,
            password: "password123",
            passwordHasher
        ).Value;
    }
}
