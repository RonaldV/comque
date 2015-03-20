namespace Comque
{
    public enum ResultStatus : byte
    {
        None = 0,
        Success,
        NotFound,
        Forbidden,
        InvalidInput,
        InvalidOutput,
        Error
    }

    public class Result<TContent>
    {
        public Result(ResultStatus status, string message = null, TContent content = default(TContent))
        {
            Status = status;
            Message = message;
            Content = content;
        }

        public ResultStatus Status { get; private set; }
        public TContent Content { get; private set; }
        public string Message { get; private set; }

        public static Result<TContent> Success()
        {
            return new Result<TContent>(ResultStatus.Success);
        }

        public static Result<TContent> Success(TContent content)
        {
            return new Result<TContent>(ResultStatus.Success, content: content);
        }

        public static Result<TContent> NotFound(string message = null)
        {
            return new Result<TContent>(ResultStatus.NotFound, message);
        }

        public static Result<TContent> Forbidden(string message = null)
        {
            return new Result<TContent>(ResultStatus.Forbidden, message);
        }

        public static Result<TContent> Error(string message = null)
        {
            return new Result<TContent>(ResultStatus.Error, message);
        }
    }

    public class Result
    {
        public Result(ResultStatus status, string message = null)
        {
            Status = status;
            Message = message;
        }

        public ResultStatus Status { get; private set; }
        public string Message { get; private set; }

        public static Result Success()
        {
            return new Result(ResultStatus.Success);
        }

        public static Result NotFound(string message = null)
        {
            return new Result(ResultStatus.NotFound, message);
        }

        public static Result Forbidden(string message = null)
        {
            return new Result(ResultStatus.Forbidden, message);
        }

        public static Result Error(string message = null)
        {
            return new Result(ResultStatus.Error, message);
        }
    }
}
