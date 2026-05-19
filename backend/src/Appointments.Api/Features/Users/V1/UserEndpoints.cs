using Appointments.Api.Features.Users.V1.Contracts;
using Appointments.Api.Infrastructure.Endpoints;
using Appointments.Api.Shared;
using Appointments.Api.Shared.Authentication;
using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Users;
using Appointments.Application.Features.Users.Commands.ChangeUserStatus;
using Appointments.Application.Features.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Appointments.Api.Features.Users.V1;

public class UserEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("users")
                                .WithTags("Users");

        group.MapGet("/{id:guid}", GetById)
             .Produces<UserApiResponse>()
             .ProducesProblem(StatusCodes.Status403Forbidden)
             .ProducesProblem(StatusCodes.Status404NotFound);

        group.MapPatch("/{id:guid}/status", UpdateStatus)
             .RequireAuthorization(AuthenticationPolicies.OnlyAdmin)
             .Produces(StatusCodes.Status204NoContent)
             .ProducesProblem(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetById(
        Guid id,
        [FromServices] IQueryHandler<GetUserByIdQuery, UserResult> handler,
        ClaimsPrincipal user,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id, user.GetUserId(), user.IsAdmin());
        var result = await handler.HandleAsync(query, cancellationToken);

        return result.ToApiResult(value => Results.Ok(value.ToUserApiResponse()));
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