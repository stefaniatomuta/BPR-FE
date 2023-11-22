using Microsoft.Extensions.Logging;

namespace BPR.Mediator.Utils;

public class Result
{
    public bool Success { get; set; }
    public IList<string> Errors { get; set; }

    public Result(bool success, IList<string> error)
    {
        Success = success;
        Errors = error;
    }

    public Result(bool success, string error)
    {
        Success = success;
        Errors = new List<string>() {error};
    }

    public Result(bool success)
    {
        Success = success;
        Errors = new List<string>();
    }

    public static Result Ok(ILogger logger, string message)
    {
        logger.LogInformation(message);
        return new Result(true);
    }

    public static Result Fail()
    {
        return new Result(false);
    }

    public static Result Fail(string message, ILogger logger)
    {
        logger.LogError(message);
        return new Result(false, message);
    }

    public static Result<T> Fail<T>(IList<string> messages, ILogger logger)
    {
        foreach (var e in messages)
        {
            logger.LogError(e);
        }

        return new Result<T>(false, messages);
    }

    public static Result<T> Fail<T>(string? message, ILogger logger)
    {
        if (message != null)
        {
            logger.LogError(message);
        }

        return new Result<T>(false, message!);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true);
    }
}

public class Result<T> : Result
{
    public T? Value { get; set; }

    public Result(bool success, string error) : base(success, error)
    {
    }

    public Result(bool success, IList<string> errors) : base(success, errors)
    {
    }

    public Result(T? value, bool success, string error) : base(success, error)
    {
        Value = value;
    }

    public Result(T? value, bool success) : base(success)
    {
        Value = value;
    }
}