using Catalog.Domain.Establishments;

namespace Catalog.Api.Features.Establishments.V1;

public record EstablishmentResponse(
    string Name,
    string Address,
    string Phone
)
{
    public static EstablishmentResponse From(Establishment establishment)
    {
        return new EstablishmentResponse
        (
            establishment.CommercialName,
            establishment.Address,
            establishment.PhoneNumber
        );
    }
}