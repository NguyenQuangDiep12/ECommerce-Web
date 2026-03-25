using System.Net;
using System.Text.Json;

namespace HelloKitty.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized access: {Path}", context.Request.Path);
                await WriteJsonResponse(context, HttpStatusCode.Unauthorized, "Access denied");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception: {Path}", context.Request.Path);
                await WriteJsonResponse(context, HttpStatusCode.InternalServerError, "An error occurred, please try again later");
            }
        }

        private static async Task WriteJsonResponse(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var response = new { message };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}