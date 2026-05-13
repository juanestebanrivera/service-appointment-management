namespace Appointments.Api.Shared;

public sealed record PagedResponse<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages,
    bool HasNextPage,
    bool HasPreviousPage
);
