using Appointments.Domain.SharedKernel.ValueObjects;

namespace Appointments.Domain.Tests.Clients.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WhenEmailIsNullOrWhiteSpace_ReturnsFailure(string? email)
    {
        // Act
        var result = Email.Create(email!);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(EmailErrors.EmailRequired, result.Error);
    }

    [Theory]
    [InlineData("username")]
    [InlineData("@domain.com")]
    [InlineData("username@.com")]
    [InlineData("username@com")]
    [InlineData("username@domain..com")]
    [InlineData("username@domain.com.")]
    [InlineData("username@domain,com")]
    [InlineData("user.name+tag@domain.com")]
    public void Create_WhenEmailFormatIsInvalid_ReturnsFailure(string email)
    {
        // Act
        var result = Email.Create(email);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(EmailErrors.InvalidEmailFormat, result.Error);
    }

    [Theory]
    [InlineData("username@domain.com")]
    [InlineData("user-name@domain.com")]
    [InlineData("user_name@sub.domain.co")]
    [InlineData("user.name@sub.domain.com")]
    public void Create_WhenEmailIsValid_ReturnsSuccessAndCreatesEmail(string email)
    {
        // Act
        var result = Email.Create(email);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(email, result.Value.Value);
    }
}
