using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Appointments;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Queries;

public class GetAllAppointmentsQueryHandlerTests
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly GetAllAppointmentsQueryHandler _handler;

    public GetAllAppointmentsQueryHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _handler = new GetAllAppointmentsQueryHandler(_appointmentRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentsExist_ReturnsSuccessWithAppointmentData()
    {
        // Arrange
        var currentTime = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var appointments = new List<Appointment>
        {
            Appointment.Book(
                clientId: Guid.NewGuid(),
                serviceId: Guid.NewGuid(),
                timeRange: TimeRange.Create(
                    startTime: new(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                    endTime: new(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                    currentTime: currentTime
                ).Value,
                priceAtBooking: 100
            ).Value,
            Appointment.Book(
                clientId: Guid.NewGuid(),
                serviceId: Guid.NewGuid(),
                timeRange: TimeRange.Create(
                    startTime: new(2026, 1, 2, 10, 0, 0, TimeSpan.Zero),
                    endTime: new(2026, 1, 2, 11, 0, 0, TimeSpan.Zero),
                    currentTime: currentTime
                ).Value,
                priceAtBooking: 200
            ).Value
        };

        var query = new GetAllAppointmentsQuery();

        _appointmentRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(appointments);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        var resultList = result.Value.ToList();
        Assert.Equal(appointments.Count, resultList.Count);

        Assert.Equal(appointments[0].Id, resultList[0].Id);
        Assert.Equal(appointments[0].ClientId, resultList[0].ClientId);
        Assert.Equal(appointments[0].ServiceId, resultList[0].ServiceId);
        Assert.Equal(appointments[0].PriceAtBooking, resultList[0].PriceAtBooking);
        Assert.Equal(appointments[0].TimeRange.StartTime, resultList[0].StartTime);
        Assert.Equal(appointments[0].TimeRange.EndTime, resultList[0].EndTime);
        Assert.Equal(appointments[0].Status, resultList[0].Status);
    }

    [Fact]
    public async Task HandleAsync_WhenNoAppointmentsExist_ReturnsSuccessWithEmptyList()
    {
        // Arrange
        var query = new GetAllAppointmentsQuery();

        _appointmentRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }
}