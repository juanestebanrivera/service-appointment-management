using Appointments.Domain.Appointments;

namespace Appointments.Domain.Tests.Appointments;

public class TimeRangeTests
{
    [Fact]
    public void Create_WhenStartTimeIsInThePast_ReturnsFailure()
    {
        // Arrange
        DateTimeOffset startTime = DateTimeOffset.Parse("2026-01-01T10:00:00Z");
        DateTimeOffset endTime = startTime.AddHours(1);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2026-02-01T00:00:00Z");

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeRangeErrors.CannotBeInThePast, result.Error);
    }

    [Theory]
    [InlineData("2026-01-01T10:00:00Z", "2026-01-01T10:00:00Z")]
    [InlineData("2026-01-02T10:00:00Z", "2026-01-01T09:00:00Z")]
    public void Create_WhenEndTimeIsBeforeOrEqualToStartTime_ReturnsFailure(string startTimeString, string endTimeString)
    {
        // Arrange
        DateTimeOffset startTime = DateTimeOffset.Parse(startTimeString);
        DateTimeOffset endTime = DateTimeOffset.Parse(endTimeString);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2026-01-01T00:00:00Z");

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(TimeRangeErrors.EndTimeMustBeAfterStartTime, result.Error);
    }

    [Theory]
    [InlineData("2026-01-01T10:00:00Z", "2026-01-01T10:04:59Z")]
    [InlineData("2026-01-01T10:00:00Z", "2026-01-01T10:05:00Z")]
    public void Create_WhenDurationIsLessThanOrEqualToFiveMinutes_ReturnsFailure(string startTimeString, string endTimeString)
    {
        // Arrange
        DateTimeOffset startTime = DateTimeOffset.Parse(startTimeString);
        DateTimeOffset endTime = DateTimeOffset.Parse(endTimeString);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2026-01-01T00:00:00Z");

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
        DateTimeOffset startTime = DateTimeOffset.Parse("2026-01-01T10:00:00Z");
        DateTimeOffset endTime = startTime.AddDays(1).AddMinutes(1);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2026-01-01T00:00:00Z");

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
        DateTimeOffset startTime = DateTimeOffset.Parse("2026-01-01T10:00:00Z");
        DateTimeOffset endTime = startTime.AddHours(1);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2026-01-01T00:00:00Z");

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
        DateTimeOffset currentTime = DateTimeOffset.Parse("2026-01-01T10:00:00Z");
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
        DateTimeOffset startTime = DateTimeOffset.Parse("2026-01-01T10:00:00Z");
        DateTimeOffset endTime = startTime.AddDays(1);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2026-01-01T00:00:00Z");

        // Act
        var result = TimeRange.Create(startTime, endTime, currentTime);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(startTime, result.Value.StartTime);
        Assert.Equal(endTime, result.Value.EndTime);
    }
}
