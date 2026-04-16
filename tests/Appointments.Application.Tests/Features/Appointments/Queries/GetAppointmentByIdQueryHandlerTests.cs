using Appointments.Application.Features.Appointments.Queries.GetAppointmentById;
using Appointments.Domain.Appointments;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Queries;

public class GetAppointmentByIdQueryHandlerTests
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly GetAppointmentByIdQueryHandler _handler;

    public GetAppointmentByIdQueryHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _handler = new GetAppointmentByIdQueryHandler(_appointmentRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var query = new GetAppointmentByIdQuery(Guid.NewGuid());

        _appointmentRepository.GetByIdAsync(query.AppointmentId, Arg.Any<CancellationToken>()).Returns((Appointment?)null);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentExists_ReturnsSuccessWithAppointmentData()
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

        var query = new GetAppointmentByIdQuery(appointment.Id);

        _appointmentRepository.GetByIdAsync(query.AppointmentId, Arg.Any<CancellationToken>()).Returns(appointment);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(appointment.Id, result.Value.Id);
        Assert.Equal(appointment.ClientId, result.Value.ClientId);
        Assert.Equal(appointment.ServiceId, result.Value.ServiceId);
        Assert.Equal(appointment.PriceAtBooking, result.Value.PriceAtBooking);
        Assert.Equal(appointment.TimeRange.StartTime, result.Value.StartTime);
        Assert.Equal(appointment.TimeRange.EndTime, result.Value.EndTime);
        Assert.Equal(appointment.Status, result.Value.Status);
    }
}