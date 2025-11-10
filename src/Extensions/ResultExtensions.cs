using Microsoft.AspNetCore.Mvc;

namespace NotesAPI.Extensions
{
    /**
     * Provides extension methods to convert application-layer Result types
     * into proper HTTP responses (IActionResult).
     */
    public static class ResultExtensions
    {
        // Converts a Result<T> into an IActionResult
        public static IActionResult ToActionResult<T>(this Result<T> result)
        {
            var response = new
            {
                isSuccess = result.IsSuccess,
                message = result.Message,
                data = result.Data,
                errors = result.Errors,
                timestamp = result.Timestamp,
                requestId = GetRequestId()
            };

            return new ObjectResult(response)
            {
                StatusCode = result.IsSuccess
                    ? StatusCodes.Status200OK
                    : MapErrorCodeToStatusCode(result.Errors.FirstOrDefault()?.Code)
            };
        }

        // Converts a Result (non-generic) into an IActionResult
        public static IActionResult ToActionResult(this Result result)
        {
            var response = new
            {
                isSuccess = result.IsSuccess,
                message = result.Message,
                data = (object?)null, // boş da olsa ortak yapı için gerekli
                errors = result.Errors,
                timestamp = result.Timestamp,
                requestId = GetRequestId()
            };

            return new ObjectResult(response)
            {
                StatusCode = result.IsSuccess
                    ? StatusCodes.Status200OK
                    : MapErrorCodeToStatusCode(result.Errors.FirstOrDefault()?.Code)
            };
        }

        // Maps custom error codes to HTTP status codes
        private static int MapErrorCodeToStatusCode(string? code) =>
            code?.ToUpperInvariant() switch
            {
                // Validation errors
                "VALIDATION_ERROR" => StatusCodes.Status400BadRequest,
                "INVALID_ARGUMENT" => StatusCodes.Status400BadRequest,
                "INVALID_OPERATION" => StatusCodes.Status400BadRequest,
                "BAD_REQUEST" => StatusCodes.Status400BadRequest,

                // Authentication & Authorization
                "UNAUTHORIZED" => StatusCodes.Status401Unauthorized,
                "FORBIDDEN" => StatusCodes.Status403Forbidden,
                "LOGIN_FAILED" => StatusCodes.Status401Unauthorized,
                "INVALID_CREDENTIALS" => StatusCodes.Status401Unauthorized,
                "TOKEN_EXPIRED" => StatusCodes.Status401Unauthorized,
                "TOKEN_INVALID" => StatusCodes.Status401Unauthorized,

                // Not found errors
                "NOT_FOUND" => StatusCodes.Status404NotFound,
                "USER_NOT_FOUND" => StatusCodes.Status404NotFound,
                "RESOURCE_NOT_FOUND" => StatusCodes.Status404NotFound,

                // Conflict errors
                "CONFLICT" => StatusCodes.Status409Conflict,
                "DUPLICATE_ENTRY" => StatusCodes.Status409Conflict,
                "USER_ALREADY_EXISTS" => StatusCodes.Status409Conflict,
                "EMAIL_ALREADY_EXISTS" => StatusCodes.Status409Conflict,

                // Business logic errors
                "PASSWORD_MISMATCH" => StatusCodes.Status400BadRequest,
                "USER_CREATION_FAILED" => StatusCodes.Status500InternalServerError,
                "INSUFFICIENT_PERMISSIONS" => StatusCodes.Status403Forbidden,
                "ACCOUNT_LOCKED" => StatusCodes.Status423Locked,
                "ACCOUNT_DISABLED" => StatusCodes.Status423Locked,

                // Server errors
                "INTERNAL_SERVER_ERROR" => StatusCodes.Status500InternalServerError,
                "DATABASE_ERROR" => StatusCodes.Status500InternalServerError,
                "EXTERNAL_SERVICE_ERROR" => StatusCodes.Status502BadGateway,
                "SERVICE_UNAVAILABLE" => StatusCodes.Status503ServiceUnavailable,

                // Default
                _ => StatusCodes.Status500InternalServerError
            };

        // Helper method to get request ID for tracking
        private static string GetRequestId()
        {
            // In a real application, you might want to use a correlation ID from the request context
            // For now, we'll use a simple timestamp-based ID
            return DateTime.UtcNow.Ticks.ToString();
        }
    }

    /**
   * Represents a standardized error model used within operation results.
   */
    public class Error
    {
        public string Code { get; set; } = default!;
        public string Message { get; set; } = default!;
        public string? Details { get; set; }
        public string? Field { get; set; }
        public string? Source { get; set; }

        // Factory method to create an error from an exception
        public static Error FromException(Exception ex) =>
            new()
            {
                Code = ex.GetType().Name,
                Message = ex.Message,
                Details = ex.StackTrace,
                Source = ex.Source ?? ex.GetType().Name
            };

        // Factory method to create a validation error
        public static Error ValidationError(string field, string message, string? details = null) =>
            new()
            {
                Code = "VALIDATION_ERROR",
                Message = message,
                Field = field,
                Details = details
            };

        // Factory method to create a business logic error
        public static Error BusinessError(string code, string message, string? details = null) =>
            new()
            {
                Code = code,
                Message = message,
                Details = details
            };

        // Factory method to create a not found error
        public static Error NotFound(string resource, string? details = null) =>
            new()
            {
                Code = "NOT_FOUND",
                Message = $"{resource} not found",
                Details = details
            };

        // Factory method to create an unauthorized error
        public static Error Unauthorized(string message = "Unauthorized access", string? details = null) =>
            new()
            {
                Code = "UNAUTHORIZED",
                Message = message,
                Details = details
            };

        // Factory method to create a forbidden error
        public static Error Forbidden(string message = "Access forbidden", string? details = null) =>
            new()
            {
                Code = "FORBIDDEN",
                Message = message,
                Details = details
            };
    }

    public class Result<T>
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public T? Data { get; private set; }
        public List<Error?> Errors { get; private set; }
        public DateTime Timestamp { get; private set; }

        protected Result(bool isSuccess, T? data, string message, List<Error?> errors)
        {
            IsSuccess = isSuccess;
            Data = data;
            Message = message;
            Errors = errors;
            Timestamp = DateTime.UtcNow;
        }

        // Creates a successful result with data
        public static Result<T> Success(T data, string message = "Operation succeeded") =>
            new(true, data, message, new());

        // Creates a failure result with a single error
        public static Result<T> Failure(string code, string message) =>
            new(false, default, message, new() { new Error { Code = code, Message = message } });

        // Creates a failure result with a list of errors
        public static Result<T> Failure(List<Error> errors, string message = "Operation failed") =>
            new(false, default, message, errors.Cast<Error?>().ToList());

        // Creates a failure result from an exception
        public static Result<T> Failure(Exception ex, string message = "An unexpected error occurred") =>
            new(false, default, message, new() { Error.FromException(ex) });

        // Allows implicit conversion from data to Result<T>
        public static implicit operator Result<T>(T data) =>
            data == null
                ? Failure("DATA_NULL", "Data is null")
                : Success(data);

        // Allows checking result success directly in if-statements
        public static implicit operator bool(Result<T> result) => result.IsSuccess;
    }

    /**
    * Represents the result of an operation without returning data.
    * Suitable for operations like create, delete, or update actions.
    */
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }
        public List<Error> Errors { get; private set; }
        public DateTime Timestamp { get; private set; }

        protected Result(bool isSuccess, string message, List<Error> errors)
        {
            IsSuccess = isSuccess;
            Message = message;
            Errors = errors;
            Timestamp = DateTime.UtcNow;
        }

        // Creates a successful result
        public static Result Success(string message = "Operation succeeded") =>
            new(true, message, new());

        // Creates a failure result with a single error
        public static Result Failure(string code, string message) =>
            new(false, message, new() { new Error { Code = code, Message = message } });

        // Creates a failure result with a list of errors
        public static Result Failure(List<Error> errors, string message = "Operation failed") =>
            new(false, message, errors);

        // Creates a failure result from an exception
        public static Result Failure(Exception ex, string message = "An unexpected error occurred") =>
            new(false, message, new() { Error.FromException(ex) });

        // Allows checking result success directly in if-statements
        public static implicit operator bool(Result result) => result.IsSuccess;
    }
}