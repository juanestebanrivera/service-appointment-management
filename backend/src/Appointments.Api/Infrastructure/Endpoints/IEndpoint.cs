namespace Appointments.Api.Infrastructure.Endpoints;

internal interface IEndpoint
{
    void MapEndpoints(IEndpointRouteBuilder routeBuilder);
}