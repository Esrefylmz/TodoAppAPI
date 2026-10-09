using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TodoAppAPI.ExceptionHandling.Exceptions;

namespace TodoAppAPI.ExceptionHandling
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public GlobalExceptionHandler(
            IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var statusCode = exception switch
            {
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };

            var title = exception switch
            {
                NotFoundException => "Not Found",
                ConflictException => "Conflict",
                UnauthorizedException => "Unauthorized",
                ValidationException => "Validation Error",
                _ => "Internal Server Error"
            };

            var detail = exception switch
            {
                NotFoundException => exception.Message,
                ConflictException => exception.Message,
                UnauthorizedException => exception.Message,
                ValidationException => "One or more validation errors occurred.",
                _ => "An unexpected error occurred."
            };

            httpContext.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail
            };

            if (exception is ValidationException validationException)
            {
                problemDetails.Extensions["errors"] =
                    validationException.Errors
                        .GroupBy(x => x.PropertyName)
                        .ToDictionary(
                            group => group.Key,
                            group => group
                                .Select(x => x.ErrorMessage)
                                .ToArray()
                        );
            }

            return await _problemDetailsService.TryWriteAsync(
                new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = problemDetails,
                    Exception = exception
                });
        }
    }
}