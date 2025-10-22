using FluentValidation;
using System.Text.Json;

namespace TriInkom.Middleware
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ValidationMiddleware> _logger;

        public ValidationMiddleware(RequestDelegate next, ILogger<ValidationMiddleware> logger)
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
            catch (ValidationException vex)
            {
                _logger.LogWarning("Ошибка валидации данных: {errors}", vex.Errors);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var failures = vex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }).ToList();
                var json = JsonSerializer.Serialize(new { errors = failures });
                await context.Response.WriteAsync(json);
            }
        }
    }
}
