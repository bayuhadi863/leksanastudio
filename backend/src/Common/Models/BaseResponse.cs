namespace LeksanaStudio.Common.Models
{
    public class BaseResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public object? Errors { get; set; }
        public string Code { get; set; } = "SUCCESS";

        public static BaseResponse<T> Ok(
            T data,
            string message = "Success",
            string code = "SUCCESS"
        )
        {
            return new BaseResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Code = code,
            };
        }

        public static BaseResponse<T> Fail(
            string message,
            string code = "ERROR",
            object? errors = null
        )
        {
            return new BaseResponse<T>
            {
                Success = false,
                Message = message,
                Errors = errors,
                Code = code,
            };
        }
    }
}
