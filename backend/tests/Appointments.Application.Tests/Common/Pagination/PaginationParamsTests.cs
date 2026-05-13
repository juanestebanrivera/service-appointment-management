using Appointments.Application.Common.Pagination;

namespace Appointments.Application.Tests.Common.Pagination;

public class PaginationParamsTests
{
    [Fact]
    public void Constructor_WithValidPageAndPageSize_UsesProvidedValues()
    {
        // Arrange
        int page = 3;
        int pageSize = 25;

        // Act
        var pagination = new PaginationParams(page, pageSize);

        // Assert
        Assert.Equal(page, pagination.Page);
        Assert.Equal(pageSize, pagination.PageSize);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithNonPositivePage_InitializesToDefaultPage(int invalidPage)
    {
        // Arrange
        int pageSize = 10;

        // Act
        var pagination = new PaginationParams(invalidPage, pageSize);

        // Assert
        Assert.Equal(PaginationParams.DefaultPage, pagination.Page);
        Assert.Equal(pageSize, pagination.PageSize);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithNonPositivePageSize_InitializesToDefaultPageSize(int invalidPageSize)
    {
        // Arrange
        int page = 2;

        // Act
        var pagination = new PaginationParams(page, invalidPageSize);

        // Assert
        Assert.Equal(PaginationParams.DefaultPageSize, pagination.PageSize);
        Assert.Equal(page, pagination.Page);
    }

    [Theory]
    [InlineData(101)]
    [InlineData(200)]
    [InlineData(1000)]
    public void Constructor_WithPageSizeExceedingMax_InitializesToMaxPageSize(int oversizedPageSize)
    {
        // Arrange
        int page = 2;

        // Act
        var pagination = new PaginationParams(page, oversizedPageSize);

        // Assert
        Assert.Equal(PaginationParams.MaxPageSize, pagination.PageSize);
        Assert.Equal(page, pagination.Page);
    }

    [Fact]
    public void Constructor_WithPageSizeEqualToMax_UsesMaxPageSize()
    {
        // Arrange
        int page = 2;

        // Act
        var pagination = new PaginationParams(page, PaginationParams.MaxPageSize);

        // Assert
        Assert.Equal(PaginationParams.MaxPageSize, pagination.PageSize);
        Assert.Equal(page, pagination.Page);
    }
}
