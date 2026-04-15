using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Appointments.Commands.BookAppointment;
using Appointments.Domain.Appointments;
using Appointments.Domain.Clients;
using Appointments.Domain.Services;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Appointments.Commands;

public class BookAppointmentCommandHandlerTests
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BookAppointmentCommandHandler _handler;

    public BookAppointmentCommandHandlerTests()
    {
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _clientRepository = Substitute.For<IClientRepository>();
        _serviceRepository = Substitute.For<IServiceRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        var timeProvider = new FakeTimeProvider();
        timeProvider.SetUtcNow(new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero));

        _handler = new BookAppointmentCommandHandler(
            _appointmentRepository,
            _clientRepository,
            _serviceRepository,
            _unitOfWork,
            timeProvider
        );
    }

    [Fact]
    public async Task HandleAsync_WhenClientDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new BookAppointmentCommand(
            ClientId: Guid.NewGuid(),
            ServiceId: Guid.NewGuid(),
            StartTime: new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero)
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns((Client?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Add(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenClientIsInactive_ReturnsFailure()
    {
        // Arrange
        var client = CreateValidClient();
        client.Deactivate();

        var command = new BookAppointmentCommand(
            ClientId: client.Id,
            ServiceId: Guid.NewGuid(),
            StartTime: new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero)
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Add(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenServiceDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var client = CreateValidClient();
        var command = new BookAppointmentCommand(
            ClientId: client.Id,
            ServiceId: Guid.NewGuid(),
            StartTime: new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero)
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);
        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns((Service?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Add(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenServiceIsInactive_ReturnsFailure()
    {
        // Arrange
        var client = CreateValidClient();
        var service = CreateValidService();
        service.Deactivate();

        var command = new BookAppointmentCommand(
            ClientId: client.Id,
            ServiceId: service.Id,
            StartTime: new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero)
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);
        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns(service);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Add(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenTimeIsInvalid_ReturnsFailure()
    {
        // Arrange
        var client = CreateValidClient();
        var service = CreateValidService();

        var command = new BookAppointmentCommand(
            ClientId: client.Id,
            ServiceId: service.Id,
            StartTime: new DateTimeOffset(2025, 12, 31, 10, 0, 0, TimeSpan.Zero)
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);
        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns(service);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _appointmentRepository.DidNotReceive().Add(Arg.Any<Appointment>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenAllDataIsValid_ReturnsSuccessAndBooksAppointment()
    {
        // Arrange
        Appointment? bookedAppointment = null;
        var client = CreateValidClient();
        var service = CreateValidService();

        var command = new BookAppointmentCommand(
            ClientId: client.Id,
            ServiceId: service.Id,
            StartTime: new DateTimeOffset(2026, 1, 2, 10, 0, 0, TimeSpan.Zero)
        );

        _clientRepository.GetByIdAsync(command.ClientId, Arg.Any<CancellationToken>()).Returns(client);
        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns(service);
        _appointmentRepository.Add(Arg.Do<Appointment>(a => bookedAppointment = a));

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(bookedAppointment);
        Assert.NotEqual(Guid.Empty, bookedAppointment.Id);
        Assert.Equal(bookedAppointment.Id, result.Value);
        Assert.Equal(command.ClientId, bookedAppointment.ClientId);
        Assert.Equal(command.ServiceId, bookedAppointment.ServiceId);
        Assert.Equal(command.StartTime, bookedAppointment.TimeRange.StartTime);
        Assert.Equal(command.StartTime.Add(service.Duration), bookedAppointment.TimeRange.EndTime);
        Assert.Equal(service.Price, bookedAppointment.PriceAtBooking);

        _appointmentRepository.Received(1).Add(Arg.Any<Appointment>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private static Client CreateValidClient()
    {
        return Client.Register(
            PersonName.Create("FirstName", nameof(Client.FirstName)).Value,
            PersonName.Create("LastName", nameof(Client.LastName)).Value,
            PhoneNumber.Create("+1", "1234567890").Value,
            Email.Create("username@domain.com").Value
        ).Value;
    }

    private static Service CreateValidService()
    {
        return Service.Create(
            name: "Service Name",
            price: 100,
            duration: TimeSpan.FromHours(1),
            description: "Service Description",
            isActive: true
        ).Value;
    }
}