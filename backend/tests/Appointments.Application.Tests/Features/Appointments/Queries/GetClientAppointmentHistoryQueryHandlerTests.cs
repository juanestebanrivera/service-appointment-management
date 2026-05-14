using Appointments.Application.Common.Pagination;
using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Appointments.Queries;
using Appointments.Application.Features.Appointments.Queries.GetClientAppointmentHistory;
using Appointments.Application.Features.Clients;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel.ValueObjects;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Queries;

public class GetClientAppointmentHistoryQueryHandlerTests
{
    private readonly IAppointmentQueryRepository _appointmentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly GetClientAppointmentHistoryQueryHandler _handler;

    public GetClientAppointmentHistoryQueryHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentQueryRepository>();
        _clientRepository = Substitute.For<IClientRepository>();
        _handler = new GetClientAppointmentHistoryQueryHandler(_appointmentRepository, _clientRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenClientDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var query = new GetClientAppointmentHistoryQuery(ClientId: Guid.NewGuid(), Page: 1, PageSize: 10, CurrentUserId: Guid.NewGuid(), IsAdmin: false);

        _clientRepository.GetByIdAsync(query.ClientId, Arg.Any<CancellationToken>()).Returns((Client?)null);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ClientApplicationErrors.NotFound, result.Error);

        await _appointmentRepository
            .DidNotReceive()
            .GetClientAppointmentHistoryAsync(Arg.Any<Guid>(), Arg.Any<PaginationParams>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenUserIsNotOwnerAndNotAdmin_ReturnsForbidden()
    {
        // Arrange
        var client = CreateValidClient();
        var query = new GetClientAppointmentHistoryQuery(ClientId: client.Id, Page: 1, PageSize: 10, CurrentUserId: Guid.NewGuid(), IsAdmin: false);

        _clientRepository.GetByIdAsync(client.Id, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ClientApplicationErrors.Forbidden, result.Error);

        await _appointmentRepository
            .DidNotReceive()
            .GetClientAppointmentHistoryAsync(Arg.Any<Guid>(), Arg.Any<PaginationParams>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenUserIsAdminAndNotOwner_ReturnsPagedResult()
    {
        // Arrange
        var client = CreateValidClient();
        var query = new GetClientAppointmentHistoryQuery(ClientId: client.Id, Page: 1, PageSize: 10, CurrentUserId: Guid.NewGuid(), IsAdmin: true);

        _clientRepository.GetByIdAsync(client.Id, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository
            .GetClientAppointmentHistoryAsync(client.Id, Arg.Any<PaginationParams>(), Arg.Any<CancellationToken>())
            .Returns(([], 0));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task HandleAsync_WhenUserIsOwnerAndNotAdmin_ReturnsPagedResult()
    {
        // Arrange
        var client = CreateValidClient();
        var query = new GetClientAppointmentHistoryQuery(ClientId: client.Id, Page: 1, PageSize: 10, CurrentUserId: client.UserId, IsAdmin: false);

        _clientRepository.GetByIdAsync(client.Id, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository
            .GetClientAppointmentHistoryAsync(client.Id, Arg.Any<PaginationParams>(), Arg.Any<CancellationToken>())
            .Returns(([], 0));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task HandleAsync_WhenClientExistsWithAppointments_ReturnsPagedResult()
    {
        // Arrange
        var client = CreateValidClient();
        var query = new GetClientAppointmentHistoryQuery(ClientId: client.Id, Page: 1, PageSize: 10, CurrentUserId: client.UserId, IsAdmin: false);

        var items = new List<ClientAppointmentResult>
        {
            new(
                Id: Guid.NewGuid(),
                PriceAtBooking: 100,
                StartTime: new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
                EndTime: new DateTimeOffset(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
                Status: AppointmentStatus.Completed,
                ServiceId: Guid.NewGuid(),
                ServiceName: "Service Name"
            ),
            new(
                Id: Guid.NewGuid(),
                PriceAtBooking: 200,
                StartTime: new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero),
                EndTime: new DateTimeOffset(2026, 1, 2, 11, 0, 0, TimeSpan.Zero),
                Status: AppointmentStatus.Cancelled,
                ServiceId: Guid.NewGuid(),
                ServiceName: "Service Name Two"
            )
        };

        _clientRepository.GetByIdAsync(client.Id, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository
            .GetClientAppointmentHistoryAsync(client.Id, Arg.Any<PaginationParams>(), Arg.Any<CancellationToken>())
            .Returns((items, items.Count));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(items.Count, result.Value.TotalCount);
        Assert.Equal(query.Page, result.Value.Page);
        Assert.Equal(query.PageSize, result.Value.PageSize);
        Assert.Equal(items.Count, result.Value.Items.Count());
    }

    [Fact]
    public async Task HandleAsync_WhenClientExistsWithNoAppointments_ReturnsEmptyPagedResult()
    {
        // Arrange
        const int TOTAL_COUNT = 0;
        var client = CreateValidClient();
        var query = new GetClientAppointmentHistoryQuery(ClientId: client.Id, Page: 1, PageSize: 10, CurrentUserId: client.UserId, IsAdmin: false);

        _clientRepository.GetByIdAsync(client.Id, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository
            .GetClientAppointmentHistoryAsync(client.Id, Arg.Any<PaginationParams>(), Arg.Any<CancellationToken>())
            .Returns(([], TOTAL_COUNT));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(TOTAL_COUNT, result.Value.TotalCount);
        Assert.Empty(result.Value.Items);
    }

    private static Client CreateValidClient()
    {
        return Client.Register(
            PersonName.Create("FirstName", nameof(Client.FirstName)).Value,
            PersonName.Create("LastName", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+1", "1234567890").Value,
            userId: Guid.NewGuid(),
            Email.Create("username@domain.com").Value
        ).Value;
    }
}
