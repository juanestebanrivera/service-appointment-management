using Appointments.Application.Common.Interfaces;
using Appointments.Application.Common.Pagination;
using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Features.Clients.Queries.GetAllClients;

public sealed class GetAllClientsQueryHandler(IClientQueryRepository clientRepository)
    : IQueryHandler<GetAllClientsQuery, PagedResult<ClientResult>>
{
    private readonly IClientQueryRepository _clientRepository = clientRepository;

    public async Task<Result<PagedResult<ClientResult>>> HandleAsync(GetAllClientsQuery query, CancellationToken cancellationToken = default)
    {
        var pagination = new PaginationParams(query.Page, query.PageSize);

        var (items, totalCount) = await _clientRepository.GetPagedAsync(pagination, query.SearchTerm, query.Status, cancellationToken);

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
