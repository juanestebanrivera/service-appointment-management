using Appointments.Domain.SharedKernel;

namespace Appointments.Application.Common.Interfaces;

public interface IQueryHandler<in TQuery, TResult>
{
    Task<Result<TResult>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}