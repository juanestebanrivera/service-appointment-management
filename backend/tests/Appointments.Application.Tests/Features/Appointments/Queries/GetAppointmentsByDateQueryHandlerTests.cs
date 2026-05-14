using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Appointments.Queries;
using Appointments.Application.Features.Appointments.Queries.GetAppointmentsByDate;
using Appointments.Domain.Appointments;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Queries;

public class GetAppointmentsByDateQueryHandlerTests
{
    private readonly IAppointmentQueryRepository _appointmentRepository;
    private readonly GetAppointmentsByDateQueryHandler _handler;

    public GetAppointmentsByDateQueryHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentQueryRepository>();
        _handler = new GetAppointmentsByDateQueryHandler(_appointmentRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenAppointmentsExist_ReturnsSuccessWithAppointmentData()
    {
        // Arrange
        var date = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var appointments = new List<AppointmentDetailResult>
        {
            new(
                Id: Guid.NewGuid(),
                ClientId: Guid.NewGuid(),
                ClientUserId: Guid.NewGuid(),
                ClientFirstName: "FirstName",
                ClientLastName: "LastName",
                ClientEmail: "username@domain.com",
                ClientPhonePrefix: "+1",
                ClientPhoneNumber: "123456789",
                ServiceId: Guid.NewGuid(),
                ServiceName: "Service Name",
                PriceAtBooking: 100,
                StartTime: new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                EndTime: new DateTimeOffset(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                Status: AppointmentStatus.Pending
            ),
            new(
                Id: Guid.NewGuid(),
                ClientId: Guid.NewGuid(),
                ClientUserId: Guid.NewGuid(),
                ClientFirstName: "FirstName Two",
                ClientLastName: "LastName Two",
                ClientEmail: "username2@domain.com",
                ClientPhonePrefix: "+1",
                ClientPhoneNumber: "987654321",
                ServiceId: Guid.NewGuid(),
                ServiceName: "Service Name Two",
                PriceAtBooking: 200,
                StartTime: new DateTimeOffset(2026, 1, 1, 12, 0, 0, TimeSpan.Zero),
                EndTime: new DateTimeOffset(2026, 1, 1, 13, 0, 0, TimeSpan.Zero),
                Status: AppointmentStatus.Confirmed
            )
        };

        var query = new GetAppointmentsByDateQuery(date);

        _appointmentRepository.GetByDateAsync(date, Arg.Any<CancellationToken>())
                              .Returns(appointments);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        var resultList = result.Value.ToList();
        Assert.Equal(appointments.Count, resultList.Count);

        Assert.Equal(appointments[0].Id, resultList[0].Id);
        Assert.Equal(appointments[0].ClientId, resultList[0].ClientId);
        Assert.Equal(appointments[0].ClientUserId, resultList[0].ClientUserId);
        Assert.Equal(appointments[0].ClientFirstName, resultList[0].ClientFirstName);
        Assert.Equal(appointments[0].ClientLastName, resultList[0].ClientLastName);
        Assert.Equal(appointments[0].ClientEmail, resultList[0].ClientEmail);
        Assert.Equal(appointments[0].ClientPhonePrefix, resultList[0].ClientPhonePrefix);
        Assert.Equal(appointments[0].ClientPhoneNumber, resultList[0].ClientPhoneNumber);
        Assert.Equal(appointments[0].ServiceId, resultList[0].ServiceId);
        Assert.Equal(appointments[0].ServiceName, resultList[0].ServiceName);
        Assert.Equal(appointments[0].PriceAtBooking, resultList[0].PriceAtBooking);
        Assert.Equal(appointments[0].StartTime, resultList[0].StartTime);
        Assert.Equal(appointments[0].EndTime, resultList[0].EndTime);
        Assert.Equal(appointments[0].Status, resultList[0].Status);
    }

    [Fact]
    public async Task HandleAsync_WhenNoAppointmentsExist_ReturnsSuccessWithEmptyList()
    {
        // Arrange
        var date = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        var query = new GetAppointmentsByDateQuery(date);

        _appointmentRepository.GetByDateAsync(date, Arg.Any<CancellationToken>())
                              .Returns([]);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }

    [Fact]
    public async Task HandleAsync_WhenDateIsDefault_ReturnsFailure()
    {
        // Arrange
        var query = new GetAppointmentsByDateQuery(default);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AppointmentApplicationErrors.DateIsRequired, result.Error);
    }
}
