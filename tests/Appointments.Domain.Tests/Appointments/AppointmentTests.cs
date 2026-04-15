using Appointments.Domain.Appointments;

namespace Appointments.Domain.Tests.Appointments;

public class AppointmentTests
{
    [Fact]
    public void Book_WhenClientIdIsEmpty_ReturnsFailure()
    {
        // Arrange
        Guid clientId = Guid.Empty;
        Guid serviceId = Guid.NewGuid();
        decimal priceAtBooking = 100;

        DateTimeOffset startTime = DateTimeOffset.Parse("2026-01-01T10:00:00Z");
        DateTimeOffset endTime = startTime.AddHours(1);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var timeRange = TimeRange.Create(startTime, endTime, currentTime).Value;

        // Act
        var result = Appointment.Book(clientId, serviceId, timeRange, priceAtBooking);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.ClientIsRequired, result.Error);
    }

    [Fact]
    public void Book_WhenServiceIdIsEmpty_ReturnsFailure()
    {
        // Arrange
        Guid clientId = Guid.NewGuid();
        Guid serviceId = Guid.Empty;
        decimal priceAtBooking = 100;

        DateTimeOffset startTime = DateTimeOffset.Parse("2026-01-01T10:00:00Z");
        DateTimeOffset endTime = startTime.AddHours(1);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var timeRange = TimeRange.Create(startTime, endTime, currentTime).Value;

        // Act
        var result = Appointment.Book(clientId, serviceId, timeRange, priceAtBooking);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.ServiceIsRequired, result.Error);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void Book_WhenPriceAtBookingIsZeroOrNegative_ReturnsFailure(decimal priceAtBooking)
    {
        // Arrange
        Guid clientId = Guid.NewGuid();
        Guid serviceId = Guid.NewGuid();

        DateTimeOffset startTime = DateTimeOffset.Parse("2026-01-01T10:00:00Z");
        DateTimeOffset endTime = startTime.AddHours(1);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var timeRange = TimeRange.Create(startTime, endTime, currentTime).Value;

        // Act
        var result = Appointment.Book(clientId, serviceId, timeRange, priceAtBooking);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.PriceAtBookingMustBeGreaterThanZero, result.Error);
    }

    [Fact]
    public void Book_WhenAllDataIsValid_ReturnsSuccessAndCreatesAppointment()
    {
        // Arrange
        Guid clientId = Guid.NewGuid();
        Guid serviceId = Guid.NewGuid();
        decimal priceAtBooking = 100;

        DateTimeOffset startTime = DateTimeOffset.Parse("2026-01-01T10:00:00Z");
        DateTimeOffset endTime = startTime.AddHours(1);
        DateTimeOffset currentTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var timeRange = TimeRange.Create(startTime, endTime, currentTime).Value;

        // Act
        var result = Appointment.Book(clientId, serviceId, timeRange, priceAtBooking);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        Assert.Equal(clientId, result.Value.ClientId);
        Assert.Equal(serviceId, result.Value.ServiceId);
        Assert.Equal(priceAtBooking, result.Value.PriceAtBooking);
        Assert.Equal(timeRange.StartTime, result.Value.TimeRange.StartTime);
        Assert.Equal(timeRange.EndTime, result.Value.TimeRange.EndTime);
        Assert.Equal(AppointmentStatus.Pending, result.Value.Status);
    }

    [Fact]
    public void Reschedule_WhenStatusIsCancelled_ReturnsFailure()
    {
        // Arrange
        var currentTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var appointment = CreateValidAppointment(currentTime);

        appointment.Cancel();

        var newTimeRange = TimeRange.Create(
            startTime: DateTimeOffset.Parse("2026-01-02T10:00:00Z"),
            endTime: DateTimeOffset.Parse("2026-01-02T11:00:00Z"),
            currentTime: currentTime
        ).Value;

        // Act
        var result = appointment.Reschedule(newTimeRange);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Reschedule_WhenStatusIsCompleted_ReturnsFailure()
    {
        // Arrange
        var currentTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var appointment = CreateValidAppointment(currentTime);

        appointment.Confirm();
        appointment.Complete();

        var newTimeRange = TimeRange.Create(
            startTime: DateTimeOffset.Parse("2026-01-02T10:00:00Z"),
            endTime: DateTimeOffset.Parse("2026-01-02T11:00:00Z"),
            currentTime: currentTime
        ).Value;

        // Act
        var result = appointment.Reschedule(newTimeRange);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Reschedule_WhenStatusIsNoShow_ReturnsFailure()
    {
        // Arrange
        var currentTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var appointment = CreateValidAppointment(currentTime);

        appointment.Confirm();
        appointment.MarkAsNoShow();

        var newTimeRange = TimeRange.Create(
            startTime: DateTimeOffset.Parse("2026-01-02T10:00:00Z"),
            endTime: DateTimeOffset.Parse("2026-01-02T11:00:00Z"),
            currentTime: currentTime
        ).Value;

        // Act
        var result = appointment.Reschedule(newTimeRange);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Reschedule_WhenStatusIsPending_ReturnsSuccessAndUpdatesTimeRange()
    {
        // Arrange
        var currentTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var appointment = CreateValidAppointment(currentTime);

        var newTimeRange = TimeRange.Create(
            startTime: DateTimeOffset.Parse("2026-01-02T10:00:00Z"),
            endTime: DateTimeOffset.Parse("2026-01-02T11:00:00Z"),
            currentTime: currentTime
        ).Value;

        // Act
        var result = appointment.Reschedule(newTimeRange);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newTimeRange.StartTime, appointment.TimeRange.StartTime);
        Assert.Equal(newTimeRange.EndTime, appointment.TimeRange.EndTime);
    }

    [Fact]
    public void Reschedule_WhenStatusIsConfirmed_ReturnsSuccessAndUpdatesTimeRange()
    {
        // Arrange
        var currentTime = DateTimeOffset.Parse("2024-01-01T00:00:00Z");
        var timeRange = TimeRange.Create(
            startTime: DateTimeOffset.Parse("2026-01-01T10:00:00Z"),
            endTime: DateTimeOffset.Parse("2026-01-01T11:00:00Z"),
            currentTime: currentTime
        ).Value;

        var appointment = Appointment.Book(
            clientId: Guid.NewGuid(),
            serviceId: Guid.NewGuid(),
            timeRange: timeRange,
            priceAtBooking: 100
        ).Value;

        appointment.Confirm();

        var newTimeRange = TimeRange.Create(
            startTime: DateTimeOffset.Parse("2026-01-02T10:00:00Z"),
            endTime: DateTimeOffset.Parse("2026-01-02T11:00:00Z"),
            currentTime: currentTime
        ).Value;

        // Act
        var result = appointment.Reschedule(newTimeRange);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(newTimeRange.StartTime, appointment.TimeRange.StartTime);
        Assert.Equal(newTimeRange.EndTime, appointment.TimeRange.EndTime);
    }

    [Fact]
    public void Confirm_WhenStatusIsCompleted_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();
        appointment.Complete();

        // Act
        var result = appointment.Confirm();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Confirm_WhenStatusIsCancelled_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Cancel();

        // Act
        var result = appointment.Confirm();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Confirm_WhenStatusIsNoShow_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();
        appointment.MarkAsNoShow();

        // Act
        var result = appointment.Confirm();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Confirm_WhenStatusIsAlreadyConfirmed_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();

        // Act
        var result = appointment.Confirm();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Confirm_WhenStatusIsPending_ReturnsSuccessAndUpdatesStatusToConfirmed()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act
        var result = appointment.Confirm();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(AppointmentStatus.Confirmed, appointment.Status);
    }

    [Fact]
    public void Cancel_WhenStatusIsCompleted_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();
        appointment.Complete();

        // Act
        var result = appointment.Cancel();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Cancel_WhenStatusIsNoShow_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();
        appointment.MarkAsNoShow();

        // Act
        var result = appointment.Cancel();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Cancel_WhenStatusIsAlreadyCancelled_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Cancel();

        // Act
        var result = appointment.Cancel();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Cancel_WhenStatusIsPending_ReturnsSuccessAndUpdatesStatusToCancelled()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act
        var result = appointment.Cancel();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
    }

    [Fact]
    public void Cancel_WhenStatusIsConfirmed_ReturnsSuccessAndUpdatesStatusToCancelled()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();

        // Act
        var result = appointment.Cancel();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
    }

    [Fact]
    public void Complete_WhenStatusIsPending_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act
        var result = appointment.Complete();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Complete_WhenStatusIsCancelled_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Cancel();

        // Act
        var result = appointment.Complete();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Complete_WhenStatusIsNoShow_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();
        appointment.MarkAsNoShow();

        // Act
        var result = appointment.Complete();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Complete_WhenStatusIsAlreadyCompleted_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();
        appointment.Complete();

        // Act
        var result = appointment.Complete();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void Complete_WhenStatusIsConfirmed_ReturnsSuccessAndUpdatesStatusToCompleted()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();

        // Act
        var result = appointment.Complete();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(AppointmentStatus.Completed, appointment.Status);
    }

    [Fact]
    public void MarkAsNoShow_WhenStatusIsPending_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        // Act
        var result = appointment.MarkAsNoShow();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void MarkAsNoShow_WhenStatusIsCancelled_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Cancel();

        // Act
        var result = appointment.MarkAsNoShow();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void MarkAsNoShow_WhenStatusIsCompleted_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();
        appointment.Complete();

        // Act
        var result = appointment.MarkAsNoShow();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void MarkAsNoShow_WhenStatusIsAlreadyNoShow_ReturnsFailure()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();
        appointment.MarkAsNoShow();

        // Act
        var result = appointment.MarkAsNoShow();

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentErrors.InvalidStatusTransition, result.Error);
    }

    [Fact]
    public void MarkAsNoShow_WhenStatusIsConfirmed_ReturnsSuccessAndUpdatesStatusToNoShow()
    {
        // Arrange
        var appointment = CreateValidAppointment();

        appointment.Confirm();

        // Act
        var result = appointment.MarkAsNoShow();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(AppointmentStatus.NoShow, appointment.Status);
    }

    private static Appointment CreateValidAppointment(DateTimeOffset? currentTime = null)
    {
        var timeRange = TimeRange.Create(
            startTime: DateTimeOffset.Parse("2026-01-01T10:00:00Z"),
            endTime: DateTimeOffset.Parse("2026-01-01T11:00:00Z"),
            currentTime: currentTime ?? DateTimeOffset.Parse("2024-01-01T00:00:00Z")
        ).Value;

        return Appointment.Book(
            clientId: Guid.NewGuid(),
            serviceId: Guid.NewGuid(),
            timeRange: timeRange,
            priceAtBooking: 100
        ).Value;
    }
}