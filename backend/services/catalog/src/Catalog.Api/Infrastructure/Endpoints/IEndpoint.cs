namespace Catalog.Api.Infrastructure.Endpoints;

public interface IEndpoint
{
    void MapEndpoints(IEndpointRouteBuilder builder);
}