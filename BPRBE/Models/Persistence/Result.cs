namespace BPRBE.Models.Persistence;

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
        Errors = new List<string>() { error };
    }

    public Result(bool success)
    {
        Success = success;
        Errors = new List<string>();
    }

    public static Result Ok()
    {
        return new Result(true);
    }

    public static Result Fail()
    {
        return new Result(false);
    }

    public static Result Fail(string message)
    {
        return new Result(false, message);
    }

    public static Result<T> Fail<T>(string message)
    {
        return new Result<T>(false, message);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true);
    }
}

public class Result<T> : Result
{
    public T? Val { get; set; }

    public Result(bool success, string error) : base(success, error)
    {

    }

    public Result(T? value, bool success, string error) : base(success, error)
    {
        Val = value;
    }

    public Result(T? value, bool success) : base(success)
    {
        Val = value;
    }
}
