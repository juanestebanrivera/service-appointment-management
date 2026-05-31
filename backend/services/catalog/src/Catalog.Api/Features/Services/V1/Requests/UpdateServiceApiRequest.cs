namespace Catalog.Api.Features.Services.V1.Requests;

public record UpdateServiceApiRequest(
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    int Minutes
);