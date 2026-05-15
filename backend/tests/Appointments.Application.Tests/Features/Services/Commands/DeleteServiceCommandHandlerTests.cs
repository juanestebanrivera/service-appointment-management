using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Services;
using Appointments.Application.Features.Services.Commands.DeleteService;
using Appointments.Domain.Appointments;
using Appointments.Domain.Services;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Services.Commands;

public class DeleteServiceCommandHandlerTests
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly DeleteServiceCommandHandler _handler;

    public DeleteServiceCommandHandlerTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        _appointmentRepository = Substitute.For<IAppointmentRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new DeleteServiceCommandHandler(_serviceRepository, _appointmentRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenServiceDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteServiceCommand(Guid.NewGuid());

        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns((Service?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceApplicationErrors.NotFound, result.Error);

        _serviceRepository.DidNotReceive().Delete(Arg.Any<Service>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenServiceHasAppointments_ReturnsHasAppointments()
    {
        // Arrange
        var service = CreateValidService();
        var command = new DeleteServiceCommand(service.Id);

        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns(service);
        _appointmentRepository.ExistsByServiceAsync(service.Id, Arg.Any<CancellationToken>()).Returns(true);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ServiceApplicationErrors.HasAppointments, result.Error);

        _serviceRepository.DidNotReceive().Delete(Arg.Any<Service>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenServiceExistsAndHasNoAppointments_ReturnsSuccessAndDeletesService()
    {
        // Arrange
        var service = CreateValidService();
        var command = new DeleteServiceCommand(service.Id);

        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns(service);
        _appointmentRepository.ExistsByServiceAsync(service.Id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);

        _serviceRepository.Received(1).Delete(service);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
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