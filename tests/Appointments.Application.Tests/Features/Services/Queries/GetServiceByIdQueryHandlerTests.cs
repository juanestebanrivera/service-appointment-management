using Appointments.Application.Features.Services.Queries.GetServiceById;
using Appointments.Domain.Services;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Services.Queries;

public class GetServiceByIdQueryHandlerTests
{
    private IServiceRepository _serviceRepository;
    private GetServiceByIdQueryHandler _handler;

    public GetServiceByIdQueryHandlerTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        _handler = new GetServiceByIdQueryHandler(_serviceRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenServiceDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var query = new GetServiceByIdQuery(Guid.NewGuid());

        _serviceRepository.GetByIdAsync(query.ServiceId, Arg.Any<CancellationToken>()).Returns((Service?)null);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async Task HandleAsync_WhenServiceExists_ReturnsSuccessWithServiceData()
    {
        // Arrange
        var service = Service.Create(
            name: "Service Name",
            price: 100,
            duration: TimeSpan.FromHours(1),
            description: "Service Description"
        ).Value;

        var query = new GetServiceByIdQuery(service.Id);

        _serviceRepository.GetByIdAsync(query.ServiceId, Arg.Any<CancellationToken>()).Returns(service);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(service.Id, result.Value.Id);
        Assert.Equal(service.Name, result.Value.Name);
        Assert.Equal(service.Description, result.Value.Description);
        Assert.Equal(service.Price, result.Value.Price);
        Assert.Equal(service.Duration, result.Value.Duration);
        Assert.Equal(service.IsActive, result.Value.IsActive);
    }
}
