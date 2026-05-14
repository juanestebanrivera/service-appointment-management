using Appointments.Application.Features.Appointments;
using Appointments.Application.Features.Appointments.Queries;
using Appointments.Application.Features.Appointments.Queries.GetClientUpcomingAppointment;
using Appointments.Application.Features.Clients;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel.ValueObjects;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Queries;

public class GetClientUpcomingAppointmentQueryHandlerTests
{
    private readonly IAppointmentQueryRepository _appointmentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly GetClientUpcomingAppointmentQueryHandler _handler;

    public GetClientUpcomingAppointmentQueryHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentQueryRepository>();
        _clientRepository = Substitute.For<IClientRepository>();
        _handler = new GetClientUpcomingAppointmentQueryHandler(_appointmentRepository, _clientRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenClientDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var query = new GetClientUpcomingAppointmentQuery(ClientId: Guid.NewGuid(), IncludeLast: false);

        _clientRepository.GetByIdAsync(query.ClientId, Arg.Any<CancellationToken>()).Returns((Client?)null);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ClientApplicationErrors.NotFound, result.Error);

        await _appointmentRepository.DidNotReceive().GetClientUpcomingAppointmentAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
        await _appointmentRepository.DidNotReceive().GetClientLastCompletedAppointmentAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenIncludeLastIsFalse_ReturnsOnlyNextAppointment()
    {
        // Arrange
        var client = CreateValidClient();
        var query = new GetClientUpcomingAppointmentQuery(ClientId: client.Id, IncludeLast: false);

        var nextAppointment = CreateClientAppointmentResult(AppointmentStatus.Confirmed);

        _clientRepository.GetByIdAsync(client.Id, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository.GetClientUpcomingAppointmentAsync(client.Id, Arg.Any<CancellationToken>()).Returns(nextAppointment);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(nextAppointment, result.Value.NextAppointment);
        Assert.Null(result.Value.LastAppointment);

        await _appointmentRepository.DidNotReceive()
            .GetClientLastCompletedAppointmentAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenIncludeLastIsTrue_ReturnsNextAndLastAppointment()
    {
        // Arrange
        var client = CreateValidClient();
        var query = new GetClientUpcomingAppointmentQuery(ClientId: client.Id, IncludeLast: true);

        var nextAppointment = CreateClientAppointmentResult(AppointmentStatus.Confirmed);
        var lastAppointment = CreateClientAppointmentResult(AppointmentStatus.Completed);

        _clientRepository.GetByIdAsync(client.Id, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository.GetClientUpcomingAppointmentAsync(client.Id, Arg.Any<CancellationToken>()).Returns(nextAppointment);
        _appointmentRepository.GetClientLastCompletedAppointmentAsync(client.Id, Arg.Any<CancellationToken>()).Returns(lastAppointment);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(nextAppointment, result.Value.NextAppointment);
        Assert.Equal(lastAppointment, result.Value.LastAppointment);
    }

    [Fact]
    public async Task HandleAsync_WhenClientHasNoUpcomingOrLastAppointment_ReturnsSuccessWithNulls()
    {
        // Arrange
        var client = CreateValidClient();
        var query = new GetClientUpcomingAppointmentQuery(ClientId: client.Id, IncludeLast: true);

        _clientRepository.GetByIdAsync(client.Id, Arg.Any<CancellationToken>()).Returns(client);
        _appointmentRepository.GetClientUpcomingAppointmentAsync(client.Id, Arg.Any<CancellationToken>()).Returns((ClientAppointmentResult?)null);
        _appointmentRepository.GetClientLastCompletedAppointmentAsync(client.Id, Arg.Any<CancellationToken>()).Returns((ClientAppointmentResult?)null);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Null(result.Value.NextAppointment);
        Assert.Null(result.Value.LastAppointment);
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

    private static ClientAppointmentResult CreateClientAppointmentResult(AppointmentStatus status)
    {
        return new ClientAppointmentResult(
            Id: Guid.NewGuid(),
            PriceAtBooking: 100,
            StartTime: new DateTimeOffset(2026, 1, 1, 10, 0, 0, TimeSpan.Zero),
            EndTime: new DateTimeOffset(2026, 1, 1, 11, 0, 0, TimeSpan.Zero),
            Status: status,
            ServiceId: Guid.NewGuid(),
            ServiceName: "Service Name"
        );
    }
}
