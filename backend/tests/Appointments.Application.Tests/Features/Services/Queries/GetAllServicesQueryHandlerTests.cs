using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Application.Features.Services.Queries.GetAllServices;
using Appointments.Domain.Services;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Services.Queries;

public class GetAllServicesQueryHandlerTests
{
    private readonly IQueryableRepository<Service> _serviceRepository;
    private readonly GetAllServicesQueryHandler _handler;

    public GetAllServicesQueryHandlerTests()
    {
        _serviceRepository = Substitute.For<IQueryableRepository<Service>>();
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

        _serviceRepository.GetPagedAsync(Arg.Any<PaginationParams>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
                          .Returns((services, services.Count));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        var resultList = result.Value.Items.ToList();
        Assert.Equal(services.Count, resultList.Count);
        Assert.Equal(services.Count, result.Value.TotalCount);

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
        const int TOTAL_SERVICES = 0;
        var query = new GetAllServicesQuery();

        _serviceRepository.GetPagedAsync(Arg.Any<PaginationParams>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
                          .Returns(([], TOTAL_SERVICES));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.Items);
        Assert.Equal(TOTAL_SERVICES, result.Value.TotalCount);
    }

    [Fact]
    public async Task HandleAsync_WhenPaginationParamsAreProvided_ReturnsPagedResultWithCorrectMetadata()
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

        var query = new GetAllServicesQuery(Page: 1, PageSize: 1);
        var paginationParams = new PaginationParams(query.Page, query.PageSize);

        _serviceRepository.GetPagedAsync(paginationParams, Arg.Any<string?>(), Arg.Any<CancellationToken>())
                          .Returns((services.Take(1).ToList(), services.Count));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(services.Count, result.Value.TotalCount);
        Assert.Equal(paginationParams.Page, result.Value.Page);
        Assert.Equal(paginationParams.PageSize, result.Value.PageSize);
        Assert.Single(result.Value.Items);
    }
}