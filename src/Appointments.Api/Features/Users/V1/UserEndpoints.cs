using Appointments.Api.Features.Users.V1.Contracts;
using Appointments.Api.Infrastructure.Endpoints;
using Appointments.Application.Common.Interfaces;
using Appointments.Application.Features.Users.Commands.UserLogin;
using Appointments.Application.Features.Users.Commands.UserRegister;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Api.Features.Users.V1;

public class UserEndpoints : IEndpoint
{
    public void MapEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("auth")
                                .WithTags("Authentication")
                                .AllowAnonymous();

        group.MapPost("token", GetToken);
        group.MapPost("signup", Register);
    }

    private static async Task<IResult> GetToken(
        [FromBody] UserLoginApiRequest request,
        [FromServices] ICommandHandler<UserLoginCommand, AuthenticationResult> handler,
        CancellationToken cancellationToken)
    {
        var command = new UserLoginCommand(request.Email, request.Password);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(value => Results.Ok(value));
    }

    private static async Task<IResult> Register(
        [FromBody] UserRegisterApiRequest request,
        [FromServices] ICommandHandler<UserRegisterCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new UserRegisterCommand(request.Email, request.Password);
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.ToApiResult(() => Results.Created());
    }
}