using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Application.Features.Clients.Queries.GetAllClients;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel.ValueObjects;
using NSubstitute;

namespace Appointments.Application.Tests.Features.Clients.Queries;

public class GetAllClientsQueryHandlerTests
{
    private readonly IQueryableRepository<Client> _clientRepository;
    private readonly GetAllClientsQueryHandler _handler;

    public GetAllClientsQueryHandlerTests()
    {
        _clientRepository = Substitute.For<IQueryableRepository<Client>>();
        _handler = new GetAllClientsQueryHandler(_clientRepository);
    }

    [Fact]
    public async Task HandleAsync_WhenClientsExist_ReturnsSuccessWithClientData()
    {
        // Arrange
        var query = new GetAllClientsQuery();
        var clients = new List<Client>
        {
            Client.Register(
                PersonName.Create("FirstNameOne", nameof(Client.FirstName)).Value,
                PersonName.Create("LastNameOne", nameof(Client.LastName)).Value,
                PhoneNumber.Create("+1", "1234567890").Value,
                userId: Guid.NewGuid(),
                Email.Create("username1@domain.com").Value
            ).Value,
            Client.Register(
                PersonName.Create("FirstNameTwo", nameof(Client.FirstName)).Value,
                PersonName.Create("LastNameTwo", nameof(Client.LastName)).Value,
                PhoneNumber.Create("+1", "0987654321").Value,
                userId: Guid.NewGuid(),
                Email.Create("username2@domain.com").Value
            ).Value
        };

        _clientRepository.GetPagedAsync(Arg.Any<PaginationParams>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
                         .Returns((clients, clients.Count));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);

        var resultList = result.Value.Items.ToList();
        Assert.Equal(clients.Count, resultList.Count);
        Assert.Equal(clients.Count, result.Value.TotalCount);

        Assert.Equal(clients[0].Id, resultList[0].Id);
        Assert.Equal(clients[0].FirstName.Value, resultList[0].FirstName);
        Assert.Equal(clients[0].LastName.Value, resultList[0].LastName);
        Assert.Equal(clients[0].Phone.Prefix, resultList[0].PhonePrefix);
        Assert.Equal(clients[0].Phone.Number, resultList[0].PhoneNumber);
        Assert.Equal(clients[0].Email?.Value, resultList[0].Email);
        Assert.Equal(clients[0].IsActive, resultList[0].IsActive);
    }

    [Fact]
    public async Task HandleAsync_WhenNoClientsExist_ReturnsSuccessWithEmptyList()
    {
        // Arrange
        const int TOTAL_CLIENTS = 0;
        var query = new GetAllClientsQuery();

        _clientRepository.GetPagedAsync(Arg.Any<PaginationParams>(), Arg.Any<string?>(), Arg.Any<CancellationToken>())
                         .Returns(([], TOTAL_CLIENTS));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Empty(result.Value.Items);
        Assert.Equal(TOTAL_CLIENTS, result.Value.TotalCount);
    }

    [Fact]
    public async Task HandleAsync_WhenPaginationParamsAreProvided_ReturnsPagedResultWithCorrectMetadata()
    {
        // Arrange
        var clients = new List<Client>
        {
            Client.Register(
                PersonName.Create("FirstNameOne", nameof(Client.FirstName)).Value,
                PersonName.Create("LastNameOne", nameof(Client.LastName)).Value,
                PhoneNumber.Create("+1", "1234567890").Value,
                userId: Guid.NewGuid(),
                Email.Create("username1@domain.com").Value
            ).Value,
            Client.Register(
                PersonName.Create("FirstNameTwo", nameof(Client.FirstName)).Value,
                PersonName.Create("LastNameTwo", nameof(Client.LastName)).Value,
                PhoneNumber.Create("+1", "0987654321").Value,
                userId: Guid.NewGuid(),
                Email.Create("username2@domain.com").Value
            ).Value
        };

        var query = new GetAllClientsQuery(Page: 1, PageSize: 1);
        var paginationParams = new PaginationParams(query.Page, query.PageSize);

        _clientRepository.GetPagedAsync(paginationParams, Arg.Any<string?>(), Arg.Any<CancellationToken>())
                         .Returns((clients.Take(1).ToList(), clients.Count));

        // Act
        var result = await _handler.HandleAsync(query, default);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(clients.Count, result.Value.TotalCount);
        Assert.Equal(paginationParams.Page, result.Value.Page);
        Assert.Equal(paginationParams.PageSize, result.Value.PageSize);
        Assert.Single(result.Value.Items);
    }
}