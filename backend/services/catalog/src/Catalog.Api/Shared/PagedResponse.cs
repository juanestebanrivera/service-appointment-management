using Catalog.Domain.SharedKernel;

namespace Catalog.Api.Shared;

public record PagedResponse<T>(
    IEnumerable<T> Data,
    int Page,
    int Size,
    int TotalRecords,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
)
{
    public static PagedResponse<T> From<TSource>(PagedList<TSource> data, Func<TSource, T> mapper)
    {
        return new PagedResponse<T>(
            data.Data.Select(mapper),
            data.PageNumber,
            data.PageSize,
            data.TotalRecords,
            data.TotalPages,
            data.HasNextPage,
            data.HasPreviousPage
        );
    }
}