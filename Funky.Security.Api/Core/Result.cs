namespace Funky.Security.Api.Core
{
    public class Result<TData> where TData:class
    {
        public TData Data { get; set; }

        public string ErrorCode { get; set; }
        public string Error { get; set; }

        public bool Status => string.IsNullOrWhiteSpace(Error);

        public static Result<TData> Success(TData data)
        {
            return new Result<TData>
            {
                Data = data
            };
        }

        public static Result<TData> Failure(string errorCode, string errorMessage)
        {
            return new Result<TData>
            {
                ErrorCode = errorCode,
                Error = errorMessage
            };
        }
    }
}