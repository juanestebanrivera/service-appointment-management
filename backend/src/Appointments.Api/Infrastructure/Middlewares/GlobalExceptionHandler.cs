using Microsoft.AspNetCore.Diagnostics;

namespace Appointments.Api.Infrastructure.Middlewares;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Unhandled exception ocurred. RequestId: {RequestId}", httpContext.TraceIdentifier);
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = Results.Problem(
            title: "Internal Server Error",
            statusCode: StatusCodes.Status500InternalServerError,
            detail: "An unexpected error occurred. Please try again later."
        );


        await response.ExecuteAsync(httpContext);

        return true;
    }
}