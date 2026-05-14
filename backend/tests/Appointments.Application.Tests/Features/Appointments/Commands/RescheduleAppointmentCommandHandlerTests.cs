using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Appointments.Commands.RescheduleAppointment;
using Appointments.Application.Features.Clients.Queries;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel.ValueObjects;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Commands;

public class RescheduleAppointmentCommandHandlerTests
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IClientQueryRepository _clientQueryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly RescheduleAppointmentCommandHandler _handler;

    public RescheduleAppointmentCommandHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _clientQueryRepository = Substitute.For<IClientQueryRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        var timeProvider = new FakeTimeProvider();
        timeProvider.SetUtcNow(new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));

        _handler = new RescheduleAppointmentCommandHandler(_appointmentRepository, _clientQueryRepository, _unitOfWork, timeProvider);
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new RescheduleAppointmentCommand(Guid.NewGuid(), new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero), Guid.NewGuid());

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns((Appointment?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Update(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenCurrentUserIsNotOwner_ReturnsForbidden()
    {
        // Arrange
        var client = CreateClient();

        var appointment = Appointment.Book(
            clientId: client.Id,
            serviceId: Guid.NewGuid(),
            timeRange: TimeRange.Create(
                startTime: new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                endTime: new(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                currentTime: new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            ).Value,
            priceAtBooking: 100
        ).Value;

        var currentUserId = Guid.NewGuid();
        var newStartTime = appointment.TimeRange.StartTime.AddDays(1);
        var command = new RescheduleAppointmentCommand(appointment.Id, newStartTime, currentUserId);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);
        _clientQueryRepository.GetByIdAsync(appointment.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentApplicationErrors.Forbidden, result.Error);

        _appointmentRepository.DidNotReceive().Update(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenNewTimeIsInvalid_ReturnsFailure()
    {
        // Arrange
        var client = CreateClient();

        var appointment = Appointment.Book(
            clientId: client.Id,
            serviceId: Guid.NewGuid(),
            timeRange: TimeRange.Create(
                startTime: new(2025, 1, 1, 10, 0, 0, TimeSpan.Zero),
                endTime: new(2025, 1, 1, 11, 0, 0, TimeSpan.Zero),
                currentTime: new(2025, 1, 1, 0, 0, 0, TimeSpan.Zero)
            ).Value,
            priceAtBooking: 100
        ).Value;
        var newStartTime = new DateTimeOffset(2025, 1, 2, 10, 0, 0, TimeSpan.Zero);

        var command = new RescheduleAppointmentCommand(appointment.Id, newStartTime, CurrentUserId: client.UserId);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);
        _clientQueryRepository.GetByIdAsync(appointment.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Update(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentStatusIsInvalid_ReturnsFailure()
    {
        // Arrange
        var client = CreateClient();

        var appointment = Appointment.Book(
            clientId: client.Id,
            serviceId: Guid.NewGuid(),
            timeRange: TimeRange.Create(
                startTime: new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                endTime: new(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                currentTime: new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            ).Value,
            priceAtBooking: 100
        ).Value;
        appointment.Cancel();

        var newStartTime = appointment.TimeRange.StartTime.AddDays(1);
        var newEndTime = newStartTime.Add(appointment.TimeRange.Duration);
        var command = new RescheduleAppointmentCommand(appointment.Id, newStartTime, CurrentUserId: client.UserId);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);
        _clientQueryRepository.GetByIdAsync(appointment.ClientId, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository.VerifyAvailabilityAsync(newStartTime, newEndTime, appointment.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Update(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenNewTimeIsNotAvailable_ReturnsFailure()
    {
        // Arrange
        var client = CreateClient();

        var appointment = Appointment.Book(
            clientId: client.Id,
            serviceId: Guid.NewGuid(),
            timeRange: TimeRange.Create(
                startTime: new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                endTime: new(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                currentTime: new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            ).Value,
            priceAtBooking: 100
        ).Value;

        var newStartTime = appointment.TimeRange.StartTime.AddDays(1);
        var command = new RescheduleAppointmentCommand(appointment.Id, newStartTime, CurrentUserId: client.UserId);
        var newEndTime = newStartTime.Add(appointment.TimeRange.Duration);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);
        _clientQueryRepository.GetByIdAsync(appointment.ClientId, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository.VerifyAvailabilityAsync(newStartTime, newEndTime, appointment.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Update(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentIsConfirmed_ReturnsSuccessAndReschedulesAppointment()
    {
        // Arrange
        var client = CreateClient();

        var appointment = Appointment.Book(
            clientId: client.Id,
            serviceId: Guid.NewGuid(),
            timeRange: TimeRange.Create(
                startTime: new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                endTime: new(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                currentTime: new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            ).Value,
            priceAtBooking: 100
        ).Value;
        appointment.Confirm();

        var newStartTime = appointment.TimeRange.StartTime.AddDays(1);
        var command = new RescheduleAppointmentCommand(appointment.Id, newStartTime, CurrentUserId: client.UserId);
        var newEndTime = newStartTime.Add(appointment.TimeRange.Duration);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);
        _clientQueryRepository.GetByIdAsync(appointment.ClientId, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository.VerifyAvailabilityAsync(newStartTime, newEndTime, appointment.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);

        _appointmentRepository.Received(1).Update(Arg.Is<Appointment>(a =>
            a.Id == appointment.Id &&
            a.Status == AppointmentStatus.Pending &&
            a.TimeRange.StartTime == newStartTime &&
            a.TimeRange.EndTime == newEndTime
        ));

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentIsPending_ReturnsSuccessAndReschedulesAppointment()
    {
        // Arrange
        var client = CreateClient();

        var appointment = Appointment.Book(
            clientId: client.Id,
            serviceId: Guid.NewGuid(),
            timeRange: TimeRange.Create(
                startTime: new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                endTime: new(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                currentTime: new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            ).Value,
            priceAtBooking: 100
        ).Value;

        var newStartTime = appointment.TimeRange.StartTime.AddDays(1);
        var command = new RescheduleAppointmentCommand(appointment.Id, newStartTime, CurrentUserId: client.UserId);
        var newEndTime = newStartTime.Add(appointment.TimeRange.Duration);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);
        _clientQueryRepository.GetByIdAsync(appointment.ClientId, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository.VerifyAvailabilityAsync(newStartTime, newEndTime, appointment.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);

        _appointmentRepository.Received(1).Update(Arg.Is<Appointment>(a =>
            a.Id == appointment.Id &&
            a.Status == AppointmentStatus.Pending &&
            a.TimeRange.StartTime == newStartTime &&
            a.TimeRange.EndTime == newEndTime
        ));

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private static Client CreateClient()
    {
        return Client.Register(
            PersonName.Create("FirstName", nameof(Client.FirstName)).Value,
            PersonName.Create("LastName", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+1", "1234567890").Value,
            userId: Guid.NewGuid()
        ).Value;
    }
}