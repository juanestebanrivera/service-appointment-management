namespace Catalog.Api.Features.Services.V1.Requests;

public record CreateServiceApiRequest(
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    int Minutes
);