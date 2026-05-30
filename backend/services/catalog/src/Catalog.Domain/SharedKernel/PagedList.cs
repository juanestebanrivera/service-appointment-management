namespace Catalog.Domain.SharedKernel;

public class PagedList<T>(IEnumerable<T> items, int page, int size, int totalRecords)
{
    public IReadOnlyList<T> Data { get; } = items.ToList();
    public int PageNumber { get; } = page;
    public int PageSize { get; } = size;
    public int TotalRecords { get; } = totalRecords;
    public int TotalPages { get; } = (int)Math.Ceiling(totalRecords / (double)size);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}