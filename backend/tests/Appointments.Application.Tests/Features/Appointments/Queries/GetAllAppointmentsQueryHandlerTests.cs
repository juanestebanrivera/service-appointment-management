using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Appointments;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Queries;

public class GetAllAppointmentsQueryHandlerTests
{
    private readonly IQueryableRepository<Appointment> _appointmentRepository;
    private readonly GetAllAppointmentsQueryHandler _handler;

    public GetAllAppointmentsQueryHandlerTests()
    {
        _appointmentRepository = Substitute.For<IQueryableRepository<Appointment>>();
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

        _appointmentRepository.GetPagedAsync(Arg.Any<PaginationParams>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
                              .Returns((appointments, appointments.Count));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        var resultList = result.Value.Items.ToList();
        Assert.Equal(appointments.Count, resultList.Count);
        Assert.Equal(appointments.Count, result.Value.TotalCount);

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
        const int TOTAL_APPOINTMENTS = 0;
        var query = new GetAllAppointmentsQuery();

        _appointmentRepository.GetPagedAsync(Arg.Any<PaginationParams>(), Arg.Any<string?>(), Arg.Any<CancellationToken>()).Returns(([], TOTAL_APPOINTMENTS));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.Items);
        Assert.Equal(TOTAL_APPOINTMENTS, result.Value.TotalCount);
    }

    [Fact]
    public async Task HandleAsync_WhenPaginationParamsAreProvided_ReturnsPagedResultWithCorrectMetadata()
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
            ).Value,
        };

        var query = new GetAllAppointmentsQuery(Page: 1, PageSize: 1);
        var paginationParams = new PaginationParams(query.Page, query.PageSize);

        _appointmentRepository.GetPagedAsync(paginationParams, Arg.Any<string?>(), Arg.Any<CancellationToken>())
                              .Returns((appointments.Take(1).ToList(), appointments.Count));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(appointments.Count, result.Value.TotalCount);
        Assert.Equal(paginationParams.Page, result.Value.Page);
        Assert.Equal(paginationParams.PageSize, result.Value.PageSize);
        Assert.Single(result.Value.Items);
    }
}