using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Appointments.Commands.ConfirmAppointment;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Commands;

public class ConfirmAppointmentCommandHandlerTests
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConfirmAppointmentCommandHandler _handler;

    public ConfirmAppointmentCommandHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _clientRepository = Substitute.For<IClientRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new ConfirmAppointmentCommandHandler(_appointmentRepository, _clientRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new ConfirmAppointmentCommand(Guid.NewGuid(), Guid.NewGuid());

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
        var client = Client.Register(
            PersonName.Create("FirstName", nameof(Client.FirstName)).Value,
            PersonName.Create("LastName", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+1", "1234567890").Value,
            userId: Guid.NewGuid()
        ).Value;

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

        var command = new ConfirmAppointmentCommand(appointment.Id, currentUserId);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);
        _clientRepository.GetByIdAsync(appointment.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(AppointmentApplicationErrors.Forbidden, result.Error);

        _appointmentRepository.DidNotReceive().Update(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentStatusIsInvalid_ReturnsFailure()
    {
        // Arrange
        var client = Client.Register(
            PersonName.Create("FirstName", nameof(Client.FirstName)).Value,
            PersonName.Create("LastName", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+1", "1234567890").Value,
            userId: Guid.NewGuid()
        ).Value;

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

        var command = new ConfirmAppointmentCommand(appointment.Id, CurrentUserId: client.UserId);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);
        _clientRepository.GetByIdAsync(appointment.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Update(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentExistsAndStatusIsValid_ReturnsSuccessAndConfirmsAppointment()
    {
        // Arrange
        var client = Client.Register(
            PersonName.Create("FirstName", nameof(Client.FirstName)).Value,
            PersonName.Create("LastName", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+1", "1234567890").Value,
            userId: Guid.NewGuid()
        ).Value;

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

        var command = new ConfirmAppointmentCommand(appointment.Id, CurrentUserId: client.UserId);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);
        _clientRepository.GetByIdAsync(appointment.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);

        _appointmentRepository.Received(1).Update(Arg.Is<Appointment>(a =>
            a.Id == appointment.Id &&
            a.Status == AppointmentStatus.Confirmed
        ));

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}