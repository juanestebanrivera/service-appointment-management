namespace Catalog.Domain.SharedKernel;

public record PageParameters(int Page, int Size)
{
    public const int MaxPageSize = 100;
    public const int DefaultPage = 1;
    public const int DefaultPageSize = 10;

    public int Page { get; init; } = Page > 0 ? Page : DefaultPage;
    public int Size { get; init; } = Size > 0 ? Math.Min(Size, MaxPageSize) : DefaultPageSize;
}