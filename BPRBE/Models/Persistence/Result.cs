namespace BPRBE.Models.Persistence;

public class Result
{
    public bool Success { get; set; }
    public IList<string> Error { get; set; }

    public Result(bool success, IList<string> error)
    {
        Success = success;
        Error = error;
    }

    public Result(bool success, string error)
    {
        Success = success;
        Error = new List<string>();
        Error.Add(error);
    }

    public Result(bool success)
    {
        Success = success;
    }

    public static Result<T> Fail<T>(string message)
    {
        return new Result<T>(false, message);
    }

    public static Result<T> Ok<T>(T value)
    {
        return new Result<T>(value, true, string.Empty);
    }
}

public class Result<T> : Result
{
    public T Val { get; set; }

    public Result(bool success, string error) : base(success, error)
    {
        
    }

    public Result(T value, bool success, string error) : base(success, error)
    {
        Val = value;
    }

    public Result(T value, bool success) : base(success)
    {
        Val = value;
    }
}
