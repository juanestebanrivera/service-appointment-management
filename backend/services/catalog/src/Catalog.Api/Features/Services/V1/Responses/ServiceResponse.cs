using Catalog.Application.Services.Queries;

namespace Catalog.Api.Features.Services.V1.Responses;

public record ServiceResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    string Currency,
    int Minutes
)
{
    public static ServiceResponse From(ServiceResult service)
    {
        return new ServiceResponse(
            service.Id,
            service.Name,
            service.Description,
            service.Price,
            service.Currency,
            service.DurationMinutes
        );
    }
}