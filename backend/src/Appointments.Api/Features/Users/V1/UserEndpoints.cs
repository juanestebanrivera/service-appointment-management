using Appointments.Api.Features.Users.V1.Contracts;
using Appointments.Api.Infrastructure.Endpoints;
using Appointments.Api.Shared;
using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Users.Commands.ChangeUserStatus;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Features.Users.V1;

public class UserEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("users")
                                .WithTags("Users");

        group.MapPatch("/{id:guid}/status", UpdateStatus)
             .RequireAuthorization(AuthenticationPolicies.OnlyAdmin)
             .Produces(StatusCodes.Status204NoContent)
             .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateUserStatusApiRequest request,
        [FromServices] ICommandHandler<ChangeUserStatusCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new ChangeUserStatusCommand(id, request.IsActive);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(() => Results.NoContent());
    }
}