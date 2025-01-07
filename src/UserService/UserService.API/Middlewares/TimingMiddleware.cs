using System.Diagnostics;

namespace UserService.API.Middlewares;

public class TimingMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var stopWatch = Stopwatch.StartNew();

        context.Response.OnStarting(() =>
        {
            var elapMs = stopWatch.Elapsed.TotalSeconds;
            context.Response.Headers.Append("X-time", $"{elapMs}");
            return Task.CompletedTask;
        });

        await _next(context);
    }
}
