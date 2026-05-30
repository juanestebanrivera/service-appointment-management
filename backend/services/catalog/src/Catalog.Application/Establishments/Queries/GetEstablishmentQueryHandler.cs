using Catalog.Application.Abstractions;
using Catalog.Domain.Establishments;
using Catalog.Domain.SharedKernel;

namespace Catalog.Application.Establishments.Queries;

public record GetEstablishmentQuery();

public class GetEstablishmentQueryHandler(
    IEstablishmentRepository establishmentRepository
) : IQueryHandler<GetEstablishmentQuery, Establishment>
{
    public async Task<Result<Establishment>> HandleAsync(GetEstablishmentQuery query, CancellationToken cancellationToken = default)
    {
        var establishment = await establishmentRepository.GetAsync(cancellationToken);
        if (establishment == null)
            return Result<Establishment>.Failure(EstablishmentErrors.NotFound);

        return Result<Establishment>.Success(establishment);
    }
}