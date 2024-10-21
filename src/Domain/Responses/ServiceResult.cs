namespace PersonalShop.Domain.Response
{
    public class ServiceResult<TResult>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; }
        public TResult? Result { get; }
        public List<string> Errors { get; } = new List<string>();

        public ServiceResult(List<string> errors, int statusCode)
        {
            IsSuccess = false;
            Errors = errors;
            StatusCode = statusCode;
        }

        public ServiceResult(string error, int statusCode)
        {
            IsSuccess = false;
            Errors = new List<string> { error };
            StatusCode = statusCode;
        }

        public ServiceResult(TResult result, int statusCode)
        {
            IsSuccess = true;
            StatusCode = statusCode;
            Result = result;
        }

        public static ServiceResult<TResult> Success(TResult result, int statusCode = 200)
        {
            return new ServiceResult<TResult>(result, statusCode);
        }

        public static ServiceResult<TResult> Failed(List<string> errors, int statusCode = 400)
        {
            return new ServiceResult<TResult>(errors, statusCode);
        }

        public static ServiceResult<TResult> Failed(string error, int statusCode = 400)
        {
            if (string.IsNullOrEmpty(error))
            {
                return new ServiceResult<TResult>(new List<string>(), statusCode);
            }
            return new ServiceResult<TResult>(new List<string> { error }, statusCode);
        }
    }
}
