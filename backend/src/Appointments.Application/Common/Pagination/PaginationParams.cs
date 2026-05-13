namespace Appointments.Application.Common.Pagination;

public sealed record PaginationParams(int Page, int PageSize)
{
    public const int MaxPageSize = 100;
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 10;

    public int Page { get; init; } = Page > 0 ? Page : DefaultPage;
    public int PageSize { get; init; } = PageSize > 0 ? Math.Min(PageSize, MaxPageSize) : DefaultPageSize;
}
