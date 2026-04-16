using Appointments.Domain.SharedKernel;

namespace Appointments.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToApiResult(this Result result, Func<IResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess();

        return result.ToProblem();
    }

    public static IResult ToApiResult<T>(this Result<T> result, Func<T, IResult> onSuccess)
    {
        if (result.IsSuccess)
            return onSuccess(result.Value);

        return result.ToProblem();
    }

    private static IResult ToProblem(this Result result)
    {
        return Results.Problem(
            title: GetTitle(result.Error.Type),
            statusCode: GetStatusCode(result.Error.Type),
            detail: result.Error.Description
        );
    }

    private static string GetTitle(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => "Validation Error",
            ErrorType.NotFound => "Resource Not Found",
            ErrorType.Conflict => "Conflict",
            _ => "Internal Server Error"
        };
    }

    private static int GetStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}