using Appointments.Application.Common.Pagination;

namespace Appointments.Application.Tests.Common.Pagination;

public class PagedResultTests
{
    [Theory]
    [InlineData(25, 10, 3)]
    [InlineData(20, 10, 2)]
    [InlineData(0, 10, 0)]
    public void Constructor_WithTotalCountAndPageSize_CalculatesTotalPagesCorrectly(int totalCount, int pageSize, int expectedTotalPages)
    {
        // Arrange
        int page = 1;

        // Act
        var result = new PagedResult<int>([], totalCount, page, pageSize);

        // Assert
        Assert.Equal(expectedTotalPages, result.TotalPages);
    }

    [Fact]
    public void Constructor_WithFirstPage_HasNoPreviousPage()
    {
        // Arrange
        int page = 1;
        int totalCount = 30;
        int pageSize = 10;

        // Act
        var result = new PagedResult<int>([], totalCount, page, pageSize);

        // Assert
        Assert.False(result.HasPreviousPage);
    }

    [Fact]
    public void Constructor_WithPageBeyondFirst_HasPreviousPage()
    {
        // Arrange
        int page = 2;
        int totalCount = 30;
        int pageSize = 10;

        // Act
        var result = new PagedResult<int>([], totalCount, page, pageSize);

        // Assert
        Assert.True(result.HasPreviousPage);
    }

    [Fact]
    public void Constructor_WithLastPage_HasNoNextPage()
    {
        // Arrange
        int page = 3;
        int totalCount = 30;
        int pageSize = 10;

        // Act
        var result = new PagedResult<int>([], totalCount, page, pageSize);

        // Assert
        Assert.False(result.HasNextPage);
    }

    [Fact]
    public void Constructor_WithPageBeforeLast_HasNextPage()
    {
        // Arrange
        int page = 1;
        int totalCount = 30;
        int pageSize = 10;

        // Act
        var result = new PagedResult<int>([], totalCount, page, pageSize);

        // Assert
        Assert.True(result.HasNextPage);
    }
}
