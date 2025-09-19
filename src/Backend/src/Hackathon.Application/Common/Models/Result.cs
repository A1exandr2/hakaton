using System.Text.Json.Serialization;

namespace Hackathon.Application.Common.Models;

public class Result
{
    public bool Succeeded { get; }
    public string[] Errors { get; }

    [JsonConstructor]
    protected Result(bool succeeded, IEnumerable<string> errors)
    {
        Succeeded = succeeded;
        Errors = errors.ToArray();
    }

    public static Result Success() => new(true, Array.Empty<string>());
    public static Result Failure(IEnumerable<string> errors) => new(false, errors);
    public static Result Failure(string error) => new(false, [error]);
}

public class Result<T> : Result
{
    public T? Value { get; }

    [JsonConstructor]
    protected internal Result(T? value, bool succeeded, IEnumerable<string> errors)
        : base(succeeded, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(value, true, Array.Empty<string>());
    public static new Result<T> Failure(IEnumerable<string> errors) => new(default, false, errors);
    public static new Result<T> Failure(string error) => new(default, false, [error]);
}