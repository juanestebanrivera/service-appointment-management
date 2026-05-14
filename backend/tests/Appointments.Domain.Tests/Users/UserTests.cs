using Appointments.Domain.SharedKernel.ValueObjects;
using Appointments.Domain.Users;

namespace Appointments.Domain.Tests.Users;

public class UserTests
{
    private readonly FakePasswordHasher _passwordHasher = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Register_WhenPasswordIsNullOrWhiteSpace_ReturnsFailure(string? password)
    {
        // Arrange
        var email = Email.Create("user@domain.com").Value;

        // Act
        var result = User.Register(email, password!, _passwordHasher);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.PasswordRequired, result.Error);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("abc")]
    [InlineData("abcde")]
    public void Register_WhenPasswordIsShorterThanSixCharacters_ReturnsFailure(string password)
    {
        // Arrange
        var email = Email.Create("user@domain.com").Value;

        // Act
        var result = User.Register(email, password, _passwordHasher);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(UserErrors.InvalidPasswordLength, result.Error);
    }

    [Fact]
    public void Register_WhenPasswordIsValid_ReturnsSuccessAndCreatesUser()
    {
        // Arrange
        var email = Email.Create("user@domain.com").Value;
        var password = "validpassword";

        // Act
        var result = User.Register(email, password, _passwordHasher);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(email, result.Value.Email);
        Assert.NotEqual(string.Empty, result.Value.PasswordHash);
    }

    [Fact]
    public void Register_WhenCreated_DefaultsToClientRoleAndIsActive()
    {
        // Arrange
        var email = Email.Create("user@domain.com").Value;
        string password = "validpassword";

        // Act
        var result = User.Register(email, password, _passwordHasher);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(UserRole.Client, result.Value.Role);
        Assert.True(result.Value.IsActive);
    }

    [Fact]
    public void Register_WhenCreated_HashesPasswordUsingPasswordHasher()
    {
        // Arrange
        var email = Email.Create("user@domain.com").Value;
        string password = "validpassword";

        // Act
        var result = User.Register(email, password, _passwordHasher);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(_passwordHasher.Hash(password), result.Value.PasswordHash);
    }

    [Fact]
    public void Activate_WhenUserIsInactive_SetsIsActiveToTrue()
    {
        // Arrange
        var user = CreateValidUser();
        user.Deactivate();

        // Act
        user.Activate();

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void Activate_WhenUserIsAlreadyActive_KeepsIsActiveTrue()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        user.Activate();

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void Deactivate_WhenUserIsActive_SetsIsActiveToFalse()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        user.Deactivate();

        // Assert
        Assert.False(user.IsActive);
    }

    [Fact]
    public void Deactivate_WhenUserIsAlreadyInactive_KeepsIsActiveFalse()
    {
        // Arrange
        var user = CreateValidUser();
        user.Deactivate();

        // Act
        user.Deactivate();

        // Assert
        Assert.False(user.IsActive);
    }

    private static User CreateValidUser()
    {
        var email = Email.Create("user@domain.com").Value;
        return User.Register(email, "validpassword", new FakePasswordHasher()).Value;
    }

    private sealed class FakePasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => $"hashed_{password}";
        public bool Verify(string password, string passwordHash) => passwordHash == $"hashed_{password}";
    }
}
