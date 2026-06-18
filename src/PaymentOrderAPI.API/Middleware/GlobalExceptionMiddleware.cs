using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PaymentOrderAPI.API.DTOs;
using PaymentOrderAPI.Application.Common.Exceptions;

namespace PaymentOrderAPI.API.Middleware;

public class GlobalExceptionMiddleware
{
    private const string ValidationErrorCode   = "VALIDATION_ERROR";
    private const string NotFoundErrorCode     = "NOT_FOUND";
    private const string UnsupportedErrorCode  = "UNSUPPORTED_OPERATION";
    private const string InternalErrorCode     = "INTERNAL_ERROR";
    private const string InternalErrorMessage  = "An unexpected error occurred.";
    private const string LogUnhandledException = "[{Method}] Unhandled exception: {Message}";

    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next   = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            var message = string.Join(" | ", ex.Errors.Select(e => e.ErrorMessage));
            await WriteResponseAsync(context, StatusCodes.Status400BadRequest,
                new ErrorResponse(ValidationErrorCode, message));
        }
        catch (NotFoundException ex)
        {
            await WriteResponseAsync(context, StatusCodes.Status404NotFound,
                new ErrorResponse(NotFoundErrorCode, ex.Message));
        }
        catch (NotSupportedException ex)
        {
            await WriteResponseAsync(context, StatusCodes.Status400BadRequest,
                new ErrorResponse(UnsupportedErrorCode, ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, LogUnhandledException, nameof(InvokeAsync), ex.Message);
            await WriteResponseAsync(context, StatusCodes.Status500InternalServerError,
                new ErrorResponse(InternalErrorCode, InternalErrorMessage));
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, int statusCode, ErrorResponse error)
    {
        context.Response.StatusCode  = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(error);
    }
}
