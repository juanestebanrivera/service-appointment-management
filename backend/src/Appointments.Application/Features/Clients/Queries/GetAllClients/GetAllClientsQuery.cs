using Appointments.Application.Common.Pagination;

namespace Appointments.Application.Features.Clients.Queries.GetAllClients;

public record GetAllClientsQuery(
    int Page = PaginationParams.DefaultPage,
    int PageSize = PaginationParams.DefaultPageSize,
    string? SearchTerm = null,
    bool? IsActive = null)
{
    public bool Status => IsActive ?? true;
};
