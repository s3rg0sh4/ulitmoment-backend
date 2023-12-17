using System.Text.Json;
using UlitMoment.Common.HttpResponseErrors;

namespace UlitMoment.Middleware;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (HttpResponseError err)
        {
            context.Response.StatusCode = err.StatusCode;
            context.Response.ContentType = "application/json";

            var errorMessage = JsonSerializer.Serialize(new { error = err.Message });
            await context.Response.WriteAsync(errorMessage);
        }
    }
}
