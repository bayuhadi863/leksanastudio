using System.Net;
using System.Text.Json;
using LeksanaStudio.Common.Exceptions;
using LeksanaStudio.Common.Models;

namespace LeksanaStudio.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IWebHostEnvironment env
        )
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is AppException appEx)
                {
                    _logger.LogWarning(
                        ex,
                        "Handled application exception. Code: {Code}, StatusCode: {StatusCode}",
                        appEx.Code,
                        appEx.StatusCode
                    );
                }
                else
                {
                    _logger.LogError(ex, "An unhandled exception has occurred.");
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var code = "INTERNAL_SERVER_ERROR";
            var message = "An internal server error occurred.";
            object? errors = null;

            if (exception is AppException appEx)
            {
                statusCode = appEx.StatusCode;
                code = appEx.Code;
                message = appEx.Message;
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = (int)HttpStatusCode.Unauthorized;
                code = "UNAUTHORIZED";
                message = "Unauthorized access.";
            }
            // Add more specific standard exception mappings here if needed

            context.Response.StatusCode = statusCode;

            // Expose diagnostic detail only in Development to aid debugging without
            // leaking internals in production responses.
            if (_env.IsDevelopment())
            {
                errors = new
                {
                    exception.StackTrace,
                    InnerException = exception.InnerException?.Message,
                };
            }

            var response = BaseResponse<object>.Fail(message, code, errors);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            var result = JsonSerializer.Serialize(response, options);

            await context.Response.WriteAsync(result);
        }
    }
}
