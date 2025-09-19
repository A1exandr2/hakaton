namespace Hackathon.Application.Common.Models;

public static class ResultExtensions
{
    public static TResult Match<T, TResult>(
        this Result<T> result,
        Func<T, TResult> onSuccess,
        Func<string[], TResult> onFailure
    )
    {
        return result.Succeeded ? onSuccess(result.Value!) : onFailure(result.Errors);
    }
}