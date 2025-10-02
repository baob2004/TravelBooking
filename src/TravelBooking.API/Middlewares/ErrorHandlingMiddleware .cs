using System.Net;
using System.Text.Json;

namespace TravelBooking.API.Middlewares;

public sealed class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, IHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var correlationId = EnsureCorrelationId(context);

        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            // Log với correlationId
            _logger.LogError(ex, "Unhandled exception. CorrelationId={CorrelationId}", correlationId);

            await WriteProblemDetailsAsync(context, ex, correlationId);
        }
    }

    private static string EnsureCorrelationId(HttpContext ctx)
    {
        const string header = "X-Correlation-Id";
        if (!ctx.Request.Headers.TryGetValue(header, out var value) || string.IsNullOrWhiteSpace(value))
        {
            value = Guid.NewGuid().ToString("N");
            ctx.Request.Headers[header] = value;
        }
        ctx.Response.Headers[header] = value!;
        return value!;
    }

    private async Task WriteProblemDetailsAsync(HttpContext context, Exception ex, string correlationId)
    {
        var (status, title) = MapException(ex);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)status;

        var problem = new
        {
            type = "https://httpstatuses.com/" + (int)status,
            title,
            status = (int)status,
            traceId = correlationId,
            errors = (object?)null, // dành chỗ cho validation nếu muốn
            detail = _env.IsDevelopment() ? ex.ToString() : null // chỉ hiện stack ở Dev
        };

        var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = _env.IsDevelopment()
        });

        await context.Response.WriteAsync(json);
    }

    private static (HttpStatusCode status, string title) MapException(Exception ex)
    {
        // Tuỳ biến mapping theo domain của bạn:
        return ex switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
            KeyNotFoundException => (HttpStatusCode.NotFound, "Resource Not Found"),
            InvalidOperationException => (HttpStatusCode.BadRequest, "Invalid Operation"),
            ArgumentException => (HttpStatusCode.BadRequest, "Invalid Argument"),
            _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
        };
    }
}
