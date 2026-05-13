using Appointments.Api.Shared;
using Appointments.Application.Common.Pagination;

namespace Appointments.Api;

public static class PagedResultExtensions
{
    public static PagedResponse<TDestination> ToPagedResponse<TSource, TDestination>(this PagedResult<TSource> pagedResult, Func<TSource, TDestination> mapper)
    {
        return new PagedResponse<TDestination>(
            pagedResult.Items.Select(mapper),
            pagedResult.TotalCount,
            pagedResult.Page,
            pagedResult.PageSize,
            pagedResult.TotalPages,
            pagedResult.HasNextPage,
            pagedResult.HasPreviousPage
        );
    }
}
