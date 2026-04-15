using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Services.Commands.UpdateService;
using Appointments.Domain.Services;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Services.Commands;

public class UpdateServiceCommandHandlerTests
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UpdateServiceCommandHandler _handler;

    public UpdateServiceCommandHandlerTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new UpdateServiceCommandHandler(_serviceRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenServiceDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new UpdateServiceCommand(
            ServiceId: Guid.NewGuid(),
            Name: "Updated Service Name",
            Price: 150,
            Duration: TimeSpan.FromHours(1.5),
            Description: "Updated Service Description",
            IsActive: true
        );

        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns((Service?)null);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _serviceRepository.DidNotReceive().Update(Arg.Any<Service>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenNameIsInvalid_ReturnsFailure()
    {
        // Arrange
        var service = CreateValidService();

        var command = new UpdateServiceCommand(
            ServiceId: service.Id,
            Name: "",
            Price: 150,
            Duration: TimeSpan.FromHours(1.5),
            Description: "New Service Description",
            IsActive: true
        );

        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns(service);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _serviceRepository.DidNotReceive().Update(Arg.Any<Service>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenPriceIsInvalid_ReturnsFailure()
    {
        // Arrange
        var service = CreateValidService();

        var command = new UpdateServiceCommand(
            ServiceId: service.Id,
            Name: "New Service Name",
            Price: -1,
            Duration: TimeSpan.FromHours(1.5),
            Description: "New Service Description",
            IsActive: true
        );

        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns(service);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _serviceRepository.DidNotReceive().Update(Arg.Any<Service>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenDurationIsInvalid_ReturnsFailure()
    {
        // Arrange
        var service = CreateValidService();

        var command = new UpdateServiceCommand(
            ServiceId: service.Id,
            Name: "New Service Name",
            Price: 150,
            Duration: TimeSpan.FromMinutes(0),
            Description: "New Service Description",
            IsActive: true
        );

        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns(service);

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _serviceRepository.DidNotReceive().Update(Arg.Any<Service>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task HandleAsync_WhenDataIsValid_ReturnsSuccessAndUpdatesService(bool isActive)
    {
        // Arrange
        Service? updatedService = null;
        var service = CreateValidService();

        var command = new UpdateServiceCommand(
            ServiceId: service.Id,
            Name: "Neew Service Name",
            Price: 150,
            Duration: TimeSpan.FromHours(1.5),
            Description: "New Service Description",
            IsActive: isActive
        );

        _serviceRepository.GetByIdAsync(command.ServiceId, Arg.Any<CancellationToken>()).Returns(service);
        _serviceRepository.Update(Arg.Do<Service>(s => updatedService = s));

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(updatedService);
        Assert.Equal(command.ServiceId, updatedService.Id);
        Assert.Equal(command.Name, updatedService.Name);
        Assert.Equal(command.Price, updatedService.Price);
        Assert.Equal(command.Duration, updatedService.Duration);
        Assert.Equal(command.Description, updatedService.Description);
        Assert.Equal(command.IsActive, updatedService.IsActive);

        _serviceRepository.Received(1).Update(updatedService);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    private static Service CreateValidService()
    {
        return Service.Create(
            name: "Service Name",
            price: 100,
            duration: TimeSpan.FromHours(1),
            description: "Service Description"
        ).Value;
    }
}