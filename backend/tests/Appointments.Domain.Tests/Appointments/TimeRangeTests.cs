using Appointments.Domain.Appointments;

namespace Appointments.Domain.Tests.Appointments;

public class TimeRangeTests
{
    [Fact]
    public void Create_WhenStartTimeIsInThePast_ReturnsFailure()
    {
        // Arrange
        var currentTime = GetDefaultCurrentTime();
        DateTimeOffset startTime = currentTime.AddDays(-1);
        DateTimeOffset endTime = startTime.AddHours(1);

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeRangeErrors.CannotBeInThePast, result.Error);
    }

    [Theory]
    [InlineData(10, 10)]
    [InlineData(10, 9)]
    public void Create_WhenEndTimeIsBeforeOrEqualToStartTime_ReturnsFailure(int startHour, int endHour)
    {
        // Arrange
        DateTimeOffset currentTime = GetDefaultCurrentTime();
        DateTimeOffset startTime = new(2026, 1, 1, startHour, 0, 0, TimeSpan.Zero);
        DateTimeOffset endTime = new(2026, 1, 1, endHour, 0, 0, TimeSpan.Zero);

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeRangeErrors.EndTimeMustBeAfterStartTime, result.Error);
    }

    [Theory]
    [InlineData(4, 59)]
    [InlineData(5, 0)]
    public void Create_WhenDurationIsLessThanOrEqualToFiveMinutes_ReturnsFailure(int endMinutes, int endSeconds)
    {
        // Arrange
        DateTimeOffset currentTime = GetDefaultCurrentTime();
        DateTimeOffset startTime = new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero);
        DateTimeOffset endTime = new(2026, 1, 1, 10, endMinutes, endSeconds, TimeSpan.Zero);

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeRangeErrors.MustBeMoreThanFiveMinutes, result.Error);
    }

    [Fact]
    public void Create_WhenDurationIsMoreThanOneDay_ReturnsFailure()
    {
        // Arrange
        DateTimeOffset currentTime = GetDefaultCurrentTime();
        DateTimeOffset startTime = new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero);
        DateTimeOffset endTime = startTime.AddDays(1).AddMinutes(1);

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeRangeErrors.MustBeLessThanOneDay, result.Error);
    }

    [Fact]
    public void Create_WhenAllDataIsValid_ReturnsSuccessAndCreatesTimeRange()
    {
        // Arrange
        DateTimeOffset currentTime = GetDefaultCurrentTime();
        DateTimeOffset startTime = new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero);
        DateTimeOffset endTime = startTime.AddHours(1);

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(startTime, result.Value.StartTime);
        Assert.Equal(endTime, result.Value.EndTime);
    }

    [Fact]
    public void Create_WhenStartTimeIsExactlyCurrentTime_ReturnsSuccessAndCreatesTimeRange()
    {
        // Arrange
        DateTimeOffset currentTime = GetDefaultCurrentTime();
        DateTimeOffset startTime = currentTime;
        DateTimeOffset endTime = startTime.AddHours(1);

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(startTime, result.Value.StartTime);
        Assert.Equal(endTime, result.Value.EndTime);
    }

    [Fact]
    public void Create_WhenDurationIsExactlyOneDay_ReturnsSuccessAndCreatesTimeRange()
    {
        // Arrange
        DateTimeOffset currentTime = GetDefaultCurrentTime();
        DateTimeOffset startTime = new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero);
        DateTimeOffset endTime = startTime.AddDays(1);

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(startTime, result.Value.StartTime);
        Assert.Equal(endTime, result.Value.EndTime);
    }

    private static DateTimeOffset GetDefaultCurrentTime() => new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
}
