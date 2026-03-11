using Appointments.Application.Dtos.Services;
using Appointments.Application.Interfaces.Services;

namespace Appointments.Api.Endpoints;

public static class ServiceEndpoints
{
    public static void MapServiceEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/services");

        group.MapGet("/", GetAll);
        group.MapGet("/{id:guid}", GetById).WithName("GetService");
        group.MapPost("/", Create);
        group.MapPatch("/{id:guid}", Update);
        group.MapDelete("/{id:guid}", Delete);
    }

    private static async Task<IResult> GetAll(IServiceService service)
    {
        var servicesResponse = await service.GetAllAsync();
        
        return Results.Ok(servicesResponse);
    }

    private static async Task<IResult> GetById(Guid id, IServiceService service)
    {
        var serviceResponse = await service.GetByIdAsync(id);

        if (serviceResponse is null)
            return Results.NotFound();

        return Results.Ok(serviceResponse);
    }

    private static async Task<IResult> Create(CreateServiceRequest request, IServiceService service)
    {
        var serviceResponse = await service.CreateAsync(request);
        return Results.CreatedAtRoute("GetService", new { id = serviceResponse.Id }, serviceResponse);
    }

    private static async Task<IResult> Update(Guid id, UpdateServiceRequest request, IServiceService service)
    {
        var serviceResponse = await service.UpdateAsync(id, request);

        if (serviceResponse is null)
            return Results.NotFound();

        return Results.NoContent();
    }

    private static async Task<IResult> Delete(Guid id, IServiceService service)
    {
        var serviceResponse = await service.DeleteAsync(id);

        if (serviceResponse is null)
            return Results.NotFound();

        return Results.NoContent();
    }
    
}