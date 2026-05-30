using Catalog.Domain.SharedKernel;

namespace Catalog.Domain.Services;

public interface IServiceRepository
{
    Task<(IEnumerable<Service> Data, int TotalRecords)> GetPagedAsync(PageParameters pagination, string? search, CancellationToken cancellationToken = default);
    Task<Service?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null, CancellationToken cancellationToken = default);

    void Add(Service service);
    void Update(Service service);
}