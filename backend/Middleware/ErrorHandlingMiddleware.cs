using System.Net;
using System.Text.Json;

namespace TriInkom.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Произошла ошибка");
                context.Response.ContentType = "application/json";

                var errorMessage = new
                {
                    error = ex.Message,
                };

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var json = JsonSerializer.Serialize(errorMessage);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
