namespace Appointments.Api.Abstractions;

internal interface IEndpoint
{
    void MapEndpoints(IEndpointRouteBuilder routeBuilder);
}