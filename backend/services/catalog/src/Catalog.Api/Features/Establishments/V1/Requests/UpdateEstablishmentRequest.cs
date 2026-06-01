namespace Catalog.Api.Features.Establishments.V1;

public record UpdateEstablishmentRequest(
    string Name,
    string Address,
    string Phone
);