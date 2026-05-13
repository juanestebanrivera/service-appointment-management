using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Domain.Clients;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Queries.GetAllClients;

public sealed class GetAllClientsQueryHandler(IQueryableRepository<Client> clientRepository)
    : IQueryHandler<GetAllClientsQuery, PagedResult<ClientResult>>
{
    private readonly IQueryableRepository<Client> _clientRepository = clientRepository;

    public async Task<Result<PagedResult<ClientResult>>> HandleAsync(GetAllClientsQuery query, CancellationToken cancellationToken = default)
    {
        var pagination = new PaginationParams(query.Page, query.PageSize);

        var (items, totalCount) = await _clientRepository.GetPagedAsync(pagination, query.SearchTerm, cancellationToken);

        var pagedResult = new PagedResult<ClientResult>
        (
            items.Select(c => c.ToClientResult()),
            totalCount,
            pagination.Page,
            pagination.PageSize
        );

        return Result<PagedResult<ClientResult>>.Success(pagedResult);
    }
}
