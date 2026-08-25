namespace LeksanaStudio.Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string message, string code = "NOT_FOUND")
            : base(message, 404, code) { }
    }

    public class BadRequestException : AppException
    {
        public BadRequestException(string message, string code = "BAD_REQUEST")
            : base(message, 400, code) { }
    }

    public class UnauthorizedException : AppException
    {
        public UnauthorizedException(string message = "Unauthorized", string code = "UNAUTHORIZED")
            : base(message, 401, code) { }
    }

    public class ForbiddenException : AppException
    {
        public ForbiddenException(string message = "Forbidden", string code = "FORBIDDEN")
            : base(message, 403, code) { }
    }

    public class ConflictException : AppException
    {
        public ConflictException(string message, string code = "CONFLICT")
            : base(message, 409, code) { }
    }

    public class InternalServerErrorException : AppException
    {
        public InternalServerErrorException(
            string message = "Internal Server Error",
            string code = "INTERNAL_SERVER_ERROR"
        )
            : base(message, 500, code) { }
    }
}
