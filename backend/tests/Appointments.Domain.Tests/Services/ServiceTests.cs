using Appointments.Domain.Services;

namespace Appointments.Domain.Tests.Services;

public class ServiceTests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WhenNameIsNullOrWhiteSpace_ReturnsFailure(string? name)
    {
        // Arrange
        decimal price = 100;
        TimeSpan duration = TimeSpan.FromMinutes(30);
        string description = "Service description";
        bool isActive = true;

        // Act
        var result = Service.Create(name!, price, duration, description, isActive);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceErrors.NameIsRequired, result.Error);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Create_WhenPriceIsZeroOrNegative_ReturnsFailure(decimal price)
    {
        // Arrange
        string name = "Service Name";
        TimeSpan duration = TimeSpan.FromMinutes(30);
        string description = "Service description";
        bool isActive = true;

        // Act
        var result = Service.Create(name, price, duration, description, isActive);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceErrors.PriceMustBeGreaterThanZero, result.Error);
    }

    [Theory]
    [InlineData(4, 59)]
    [InlineData(5, 0)]
    public void Create_WhenDurationIsLessThanOrEqualToFiveMinutes_ReturnsFailure(int minutes, int seconds)
    {
        // Arrange
        string name = "Service Name";
        decimal price = 100;
        string description = "Service description";
        bool isActive = true;
        TimeSpan duration = TimeSpan.FromMinutes(minutes).Add(TimeSpan.FromSeconds(seconds));

        // Act
        var result = Service.Create(name, price, duration, description, isActive);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceErrors.DurationMustBeMoreThanFiveMinutes, result.Error);
    }

    [Fact]
    public void Create_WhenDurationIsGreaterThanOneDay_ReturnsFailure()
    {
        // Arrange
        string name = "Service name";
        decimal price = 100;
        TimeSpan duration = TimeSpan.FromDays(1).Add(TimeSpan.FromMinutes(1));
        string description = "Service description";
        bool isActive = true;

        // Act
        var result = Service.Create(name, price, duration, description, isActive);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceErrors.DurationMustBeLessThanOneDay, result.Error);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void Create_WhenDataIsValid_ReturnsSuccess(bool isActive)
    {
        // Arrange
        string name = "Service Name";
        decimal price = 100;
        TimeSpan duration = TimeSpan.FromMinutes(30);
        string description = "Service description";

        // Act
        var result = Service.Create(name, price, duration, description, isActive);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(name, result.Value.Name);
        Assert.Equal(price, result.Value.Price);
        Assert.Equal(duration, result.Value.Duration);
        Assert.Equal(description, result.Value.Description);
        Assert.Equal(isActive, result.Value.IsActive);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateInformation_WhenNewNameIsNullOrWhiteSpace_ReturnsFailure(string? newName)
    {
        // Arrange
        var service = CreateValidService();

        // Act
        var result = service.UpdateInformation(newName!, "New description");

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceErrors.NameIsRequired, result.Error);
    }

    [Fact]
    public void UpdateInformation_WhenDescriptionIsNull_ReturnsSuccessAndAllowsNullDescription()
    {
        // Arrange
        var service = CreateValidService();

        string newName = "New Service Name";
        string? newDescription = null;

        // Act
        var result = service.UpdateInformation(newName, newDescription);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newName, service.Name);
        Assert.Null(service.Description);
    }

    [Fact]
    public void UpdateInformation_WhenAllDataIsValid_ReturnsSuccessAndUpdatesInformation()
    {
        // Arrange
        var service = CreateValidService();

        string newName = "New Service Name";
        string newDescription = "New service description";

        // Act
        var result = service.UpdateInformation(newName, newDescription);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newName, service.Name);
        Assert.Equal(newDescription, service.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void AdjustPrice_WhenNewPriceIsZeroOrNegative_ReturnsFailure(decimal newPrice)
    {
        // Arrange
        var service = CreateValidService();

        // Act
        var result = service.AdjustPrice(newPrice);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceErrors.PriceMustBeGreaterThanZero, result.Error);
    }

    [Fact]
    public void AdjustPrice_WhenNewPriceIsValid_ReturnsSuccessAndUpdatesPrice()
    {
        // Arrange
        var service = CreateValidService();
        decimal newPrice = 150;

        // Act
        var result = service.AdjustPrice(newPrice);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newPrice, service.Price);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(4, 59)]
    public void ChangeDuration_WhenNewDurationIsLessThanOrEqualToFiveMinutes_ReturnsFailure(int minutes, int seconds = 0)
    {
        // Arrange
        var service = CreateValidService();
        TimeSpan newDuration = TimeSpan.FromMinutes(minutes).Add(TimeSpan.FromSeconds(seconds));

        // Act
        var result = service.ChangeDuration(newDuration);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceErrors.DurationMustBeMoreThanFiveMinutes, result.Error);
    }

    [Fact]
    public void ChangeDuration_WhenNewDurationIsGreaterThanOneDay_ReturnsFailure()
    {
        // Arrange
        var service = CreateValidService();
        TimeSpan newDuration = TimeSpan.FromDays(1).Add(TimeSpan.FromMinutes(1));

        // Act
        var result = service.ChangeDuration(newDuration);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceErrors.DurationMustBeLessThanOneDay, result.Error);
    }

    [Fact]
    public void ChangeDuration_WhenNewDurationIsValid_ReturnsSuccessAndUpdatesDuration()
    {
        // Arrange
        var service = CreateValidService();
        TimeSpan newDuration = TimeSpan.FromMinutes(45);

        // Act
        var result = service.ChangeDuration(newDuration);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newDuration, service.Duration);
    }

    [Fact]
    public void Activate_WhenServiceIsInactive_SetsIsActiveToTrue()
    {
        // Arrange
        var service = CreateValidService(isActive: false);

        // Act
        service.Activate();

        // Assert
        Assert.True(service.IsActive);
    }

    [Fact]
    public void Activate_WhenServiceIsAlreadyActive_KeepsIsActiveTrue()
    {
        // Arrange
        var service = CreateValidService(isActive: true);

        // Act
        service.Activate();

        // Assert
        Assert.True(service.IsActive);
    }

    [Fact]
    public void Deactivate_WhenServiceIsActive_SetsIsActiveToFalse()
    {
        // Arrange
        var service = CreateValidService(isActive: true);

        // Act
        service.Deactivate();

        // Assert
        Assert.False(service.IsActive);
    }

    [Fact]
    public void Deactivate_WhenServiceIsAlreadyInactive_KeepsIsActiveFalse()
    {
        // Arrange
        var service = CreateValidService(isActive: false);

        // Act
        service.Deactivate();

        // Assert
        Assert.False(service.IsActive);
    }

    private static Service CreateValidService(bool isActive = true)
    {
        var result = Service.Create(
            name: "Service Name",
            price: 100,
            duration: TimeSpan.FromMinutes(30),
            description: "Service description",
            isActive: isActive
        );

        return result.Value;
    }
}