using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Abstractions;

public interface IQueryHandler<in TQuery, TResult>
{
    Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}