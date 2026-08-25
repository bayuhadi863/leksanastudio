namespace LeksanaStudio.Common.Exceptions
{
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }
        public string Code { get; }

        protected AppException(string message, int statusCode, string code) : base(message)
        {
            StatusCode = statusCode;
            Code = code;
        }
    }
}
