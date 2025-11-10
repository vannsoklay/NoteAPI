using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace NotesAPI.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if request contains the header
        if (context.Request.Headers.TryGetValue("X-User-Id", out var userId))
        {
            // Save userId into HttpContext.Items
            context.Items["UserId"] = userId.ToString();
        }

        await _next(context);
    }
}
