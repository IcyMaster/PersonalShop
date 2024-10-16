namespace PersonalShop.Domain.Response
{
    public class ApiResult<TResult>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; }
        public TResult? Result { get; }
        public List<string> Errors { get; } = new List<string>();

        public ApiResult(List<string> errors, int statusCode)
        {
            IsSuccess = false;
            Errors = errors;
            StatusCode = statusCode;
        }

        public ApiResult(string error, int statusCode)
        {
            IsSuccess = false;
            Errors = new List<string> { error };
            StatusCode = statusCode;
        }

        public ApiResult(TResult result, int statusCode)
        {
            IsSuccess = true;
            StatusCode = statusCode;
            Result = result;
        }

        public static ApiResult<TResult> Success(TResult result, int statusCode = 200)
        {
            return new ApiResult<TResult>(result, statusCode);
        }

        public static ApiResult<TResult> Failed(List<string> errors, int statusCode = 400)
        {
            return new ApiResult<TResult>(errors, statusCode);
        }

        public static ApiResult<TResult> Failed(string error, int statusCode = 400)
        {
            if (string.IsNullOrEmpty(error))
            {
                return new ApiResult<TResult>(new List<string>(), statusCode);
            }
            return new ApiResult<TResult>(new List<string> { error }, statusCode);
        }
    }
}
