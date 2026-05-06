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

        var resultList = result.Value.ToList();
        Assert.Equal(services.Count, resultList.Count);

        Assert.Equal(services[0].Id, resultList[0].Id);
        Assert.Equal(services[0].Name, resultList[0].Name);
        Assert.Equal(services[0].Description, resultList[0].Description);
        Assert.Equal(services[0].Price, resultList[0].Price);
        Assert.Equal(services[0].Duration, resultList[0].Duration);
        Assert.Equal(services[0].IsActive, resultList[0].IsActive);
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