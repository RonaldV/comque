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

    public class Result<TContent> : Result
    {
        public Result(ResultStatus status, string message = null, TContent content = default(TContent))
            : base(status, message)
        {
            Content = content;
        }

        public TContent Content { get; private set; }

        public static Result<TContent> Success(TContent content)
        {
            return new Result<TContent>(ResultStatus.Success, content: content);
        }

        public new static Result<TContent> Success()
        {
            return new Result<TContent>(ResultStatus.Success);
        }

        public new static Result<TContent> NotFound(string message = null)
        {
            return new Result<TContent>(ResultStatus.NotFound, message);
        }

        public new static Result<TContent> Forbidden(string message = null)
        {
            return new Result<TContent>(ResultStatus.Forbidden, message);
        }

        public new static Result<TContent> InvalidInput(string message = null)
        {
            return new Result<TContent>(ResultStatus.InvalidInput, message);
        }

        public new static Result<TContent> InvalidOutput(string message = null)
        {
            return new Result<TContent>(ResultStatus.InvalidOutput, message);
        }

        public new static Result<TContent> Error(string message = null)
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

        public static Result InvalidInput(string message = null)
        {
            return new Result(ResultStatus.InvalidInput, message);
        }

        public static Result InvalidOutput(string message = null)
        {
            return new Result(ResultStatus.InvalidOutput, message);
        }

        public static Result Error(string message = null)
        {
            return new Result(ResultStatus.Error, message);
        }
    }
}
