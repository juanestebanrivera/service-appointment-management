using Appointments.Domain.Clients;

namespace Appointments.Domain.Tests.Clients.ValueObjects;

public class PersonNameTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WhenValueIsNullOrWhiteSpace_ReturnsFailure(string? value)
    {
        // Arrange
        string fieldName = "Name";

        // Act
        var result = PersonName.Create(value!, fieldName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PersonNameErrors.IsRequired(fieldName), result.Error);
    }

    [Theory]
    [InlineData("Name123")]
    [InlineData("123Name")]
    [InlineData("123")]
    public void Create_WhenValueContainsNumbers_ReturnsFailure(string name)
    {
        // Arrange
        string fieldName = "Name";

        // Act
        var result = PersonName.Create(name, fieldName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PersonNameErrors.CannotContainNumbers(fieldName), result.Error);
    }

    [Fact]
    public void Create_WhenValueLengthIsLessThanTwoCharacters_ReturnsFailure()
    {
        // Arrange
        string name = "A";
        string fieldName = "Name";

        // Act
        var result = PersonName.Create(name, fieldName);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(PersonNameErrors.MustBeAtLeastTwoCharacters(fieldName), result.Error);
    }

    [Theory]
    [InlineData("ValidName")]
    [InlineData("Valid-Name")]
    [InlineData("Valid Name")]
    [InlineData("Valid Name Á")]
    public void Create_WhenValueIsValid_ReturnsSuccessAndCreatesPersonName(string name)
    {
        // Arrange
        string fieldName = "Name";

        // Act
        var result = PersonName.Create(name, fieldName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(name, result.Value!.Value);
    }

    [Fact]
    public void Create_WhenValueLengthIsExactlyTwoCharacters_ReturnsSuccessAndCreatesPersonName()
    {
        // Arrange
        string name = "Na";
        string fieldName = "Name";

        // Act
        var result = PersonName.Create(name, fieldName);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(name, result.Value!.Value);
    }
}