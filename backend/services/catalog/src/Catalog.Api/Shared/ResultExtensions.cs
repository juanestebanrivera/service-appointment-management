using Catalog.Domain.SharedKernel;

namespace Catalog.Api.Shared;

public static class ResultExtensions
{
    public static IResult ToApiResult(this Result result, Func<IResult> onSuccess)
    {
        if (result.IsFailure)
            return CreateProblem(result);

        return onSuccess();
    }

    public static IResult ToApiResult<T>(this Result<T> result, Func<T, IResult> onSuccess)
    {
        if (result.IsFailure)
            return CreateProblem(result);

        return onSuccess(result.Value);
    }

    private static IResult CreateProblem(Result result)
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
            _ => "Internal Server Error"
        };
    }

    private static int GetStatusCode(ErrorType errorType)
    {
        return errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };
    }
}