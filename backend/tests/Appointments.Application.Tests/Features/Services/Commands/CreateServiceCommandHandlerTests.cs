using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Services.Commands.CreateService;
using Appointments.Domain.Services;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Services.Commands;

public class CreateServiceCommandHandlerTests
{
    private readonly IServiceRepository _serviceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly CreateServiceCommandHandler _handler;

    public CreateServiceCommandHandlerTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _handler = new CreateServiceCommandHandler(_serviceRepository, _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_WhenDataIsInvalid_ReturnsFailure()
    {
        // Arrange
        var command = new CreateServiceCommand(
            Name: "",
            Price: -1,
            Duration: TimeSpan.FromMinutes(0),
            Description: "Service Description"
        );

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);

        _serviceRepository.DidNotReceive().Add(Arg.Any<Service>());
        await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_WhenDataIsValid_ReturnsSuccessWithServiceId()
    {
        // Arrange
        Service? createdService = null;
        var command = new CreateServiceCommand(
            Name: "Service Name",
            Price: 100,
            Duration: TimeSpan.FromHours(1),
            Description: "Service Description"
        );

        _serviceRepository.Add(Arg.Do<Service>(s => createdService = s));

        // Act
        var result = await _handler.HandleAsync(command, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(createdService);
        Assert.NotEqual(Guid.Empty, createdService.Id);
        Assert.Equal(createdService.Id, result.Value);
        Assert.Equal(command.Name, createdService.Name);
        Assert.Equal(command.Price, createdService.Price);
        Assert.Equal(command.Duration, createdService.Duration);
        Assert.Equal(command.Description, createdService.Description);
        Assert.True(createdService.IsActive);

        _serviceRepository.Received(1).Add(Arg.Any<Service>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}