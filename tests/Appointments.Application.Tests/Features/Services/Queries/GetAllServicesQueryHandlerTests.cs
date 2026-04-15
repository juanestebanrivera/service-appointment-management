using Appointments.Application.Features.Services.Queries.GetAllServices;
using Appointments.Domain.Services;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Services.Queries;

public class GetAllServicesQueryHandlerTests
{
    private readonly IServiceRepository _serviceRepository;
    private readonly GetAllServicesQueryHandler _handler;

    public GetAllServicesQueryHandlerTests()
    {
        _serviceRepository = Substitute.For<IServiceRepository>();
        _handler = new GetAllServicesQueryHandler(_serviceRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenServicesExist_ReturnsSuccessWithServiceData()
    {
        // Arrange
        var services = new List<Service>
        {
            Service.Create(
                name: "Service 1",
                price: 100,
                duration: TimeSpan.FromHours(1),
                description: "Service 1 Description"
            ).Value,
            Service.Create(
                name: "Service 2",
                price: 200,
                duration: TimeSpan.FromHours(2),
                description: "Service 2 Description"
            ).Value
        };

        var query = new GetAllServicesQuery();

        _serviceRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(services);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(services.Count, result.Value.Count());

        var firstServiceResponse = result.Value.First();
        Assert.Equal(services[0].Id, firstServiceResponse.Id);
        Assert.Equal(services[0].Name, firstServiceResponse.Name);
        Assert.Equal(services[0].Description, firstServiceResponse.Description);
        Assert.Equal(services[0].Price, firstServiceResponse.Price);
        Assert.Equal(services[0].Duration, firstServiceResponse.Duration);
        Assert.Equal(services[0].IsActive, firstServiceResponse.IsActive);
    }

    [Fact]
    public async Task HandleAsync_WhenNoServicesExist_ReturnsSuccessWithEmptyList()
    {
        // Arrange
        var query = new GetAllServicesQuery();

        _serviceRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns([]);

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value);
    }
}