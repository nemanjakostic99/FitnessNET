using FitnessNET.Models;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text.Json;

namespace FitnessNET.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        //private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            //_next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context); 
            }
            catch (SecurityTokenException) 
            {
                await HandleExceptionAsync(context, HttpStatusCode.Unauthorized, "Invalid or expired token.");
            }
            catch (UnauthorizedAccessException) 
            {
                await HandleExceptionAsync(context, HttpStatusCode.Forbidden, "You do not have permission to access this resource.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unhandled Exception: {ex}");
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "An unexpected error occurred. Please try again later.");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            var errorResponse = new ErrorResponse((int)statusCode, message);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errorResponse.StatusCode;

            var jsonResponse = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}
