using Appointments.Application.Common.Pagination;

namespace Appointments.Application.Features.Services.Queries.GetAllServices;

public record GetAllServicesQuery(
    int Page = PaginationParams.DefaultPage,
    int PageSize = PaginationParams.DefaultPageSize,
    string? SearchTerm = null,
    bool? IsActive = null)
{
    public bool Status => IsActive ?? true;
};