using Appointments.Application.Dtos.Clients;
using Appointments.Application.Interfaces.Services;

namespace Appointments.Api.Endpoints;

public static class ClientEndpoints
{
    public static void MapClientEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/clients");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetClient");
        group.MapPost("/", Create);
        group.MapPatch("/{id:guid}/contact-information", UpdateContactInformation);
        group.MapDelete("/{id:guid}", Delete);
    }

    private static async Task<IResult> GetAll(IClientService service)
    {
        var clients = await service.GetAllAsync();
        
        return Results.Ok(clients);
    }

    private static async Task<IResult> GetById(Guid id, IClientService service)
    {
        var client = await service.GetByIdAsync(id);

        if (client is null)
            return Results.NotFound();

        return Results.Ok(client);
    }

    private static async Task<IResult> Create(CreateClientRequest request, IClientService service)
    {
        var client = await service.CreateAsync(request);
        return Results.CreatedAtRoute("GetClient", new { id = client.Id }, client);
    }

    private static async Task<IResult> UpdateContactInformation(Guid id, UpdateContactInformationClientRequest request, IClientService service)
    {
        var client = await service.UpdateContactInformationAsync(id, request);

        if (client is null)
            return Results.NotFound();

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, IClientService service)
    {
        var client = await service.DeleteAsync(id);

        if (client is null)
            return Results.NotFound();

        return Results.NoContent();
    }
}