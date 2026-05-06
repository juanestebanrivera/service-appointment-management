using Appointments.Domain.Clients;

namespace Appointments.Domain.Tests.Clients.ValueObjects;

public class PhoneNumberTests
{
    [Theory]
    [InlineData(null, "123456789")]
    [InlineData("", "123456789")]
    [InlineData(" ", "123456789")]
    public void Create_WhenPrefixIsNullOrWhiteSpace_ReturnsFailure(string? prefix, string number)
    {
        // Act
        var result = PhoneNumber.Create(prefix!, number);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PhoneNumberErrors.PhonePrefixRequired, result.Error);
    }

    [Theory]
    [InlineData("57", "123456789")]
    [InlineData("abc", "123456789")]
    [InlineData("+", "123456789")]
    [InlineData("+@-", "123456789")]
    [InlineData("+abc", "123456789")]
    [InlineData("+a1", "123456789")]
    public void Create_WhenPrefixFormatIsInvalid_ReturnsFailure(string prefix, string number)
    {
        // Act
        var result = PhoneNumber.Create(prefix, number);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PhoneNumberErrors.InvalidPhonePrefix, result.Error);
    }

    [Theory]
    [InlineData("+57", null)]
    [InlineData("+57", "")]
    [InlineData("+57", " ")]
    public void Create_WhenNumberIsNullOrWhiteSpace_ReturnsFailure(string prefix, string? number)
    {
        // Act
        var result = PhoneNumber.Create(prefix, number!);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PhoneNumberErrors.PhoneNumberRequired, result.Error);
    }

    [Theory]
    [InlineData("+57", "abcdefg")]
    [InlineData("+57", "123456")]
    [InlineData("+57", "1234567abc")]
    [InlineData("+57", "123 456 7890")]
    public void Create_WhenNumberFormatIsInvalid_ReturnsFailure(string prefix, string number)
    {
        // Act
        var result = PhoneNumber.Create(prefix, number);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PhoneNumberErrors.InvalidPhoneNumberFormat, result.Error);
    }

    [Theory]
    [InlineData("+57", "1234567")]
    [InlineData("+1", "9876543210")]
    public void Create_WhenPhoneNumberIsValid_ReturnsSuccessAndCreatesPhoneNumber(string prefix, string number)
    {
        // Act
        var result = PhoneNumber.Create(prefix, number);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(prefix, result.Value.Prefix);
        Assert.Equal(number, result.Value.Number);
    }
}