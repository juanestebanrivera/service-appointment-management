using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Appointments.Commands.CancelAppointment;
using Appointments.Domain.Appointments;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Commands;

public class CancelAppointmentCommandHandlerTests
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CancelAppointmentCommandHandler _handler;

    public CancelAppointmentCommandHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CancelAppointmentCommandHandler(_appointmentRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new CancelAppointmentCommand(Guid.NewGuid());

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
    public async Task HandleAsync_WhenAppointmentStatusIsInvalid_ReturnsFailure()
    {
        // Arrange
        var appointment = Appointment.Book(
            clientId: Guid.NewGuid(),
            serviceId: Guid.NewGuid(),
            timeRange: TimeRange.Create(
                startTime: new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                endTime: new(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                currentTime: new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            ).Value,
            priceAtBooking: 100
        ).Value;

        appointment.Cancel();
        var command = new CancelAppointmentCommand(appointment.Id);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Update(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentExistsAndStatusIsValid_ReturnsSuccessAndCancelsAppointment()
    {
        // Arrange
        var appointment = Appointment.Book(
            clientId: Guid.NewGuid(),
            serviceId: Guid.NewGuid(),
            timeRange: TimeRange.Create(
                startTime: new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                endTime: new(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                currentTime: new(2026, 1, 1, 0, 0, 0, TimeSpan.Zero)
            ).Value,
            priceAtBooking: 100
        ).Value;

        var command = new CancelAppointmentCommand(appointment.Id);

        _appointmentRepository.GetByIdAsync(command.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);

        _appointmentRepository.Received(1).Update(Arg.Is<Appointment>(a =>
            a.Id == appointment.Id &&
            a.Status == AppointmentStatus.Cancelled
        ));

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}