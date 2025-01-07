using UserService.API.Models;
using UserService.Domain.Entities;
using UserService.Domain.Exceptions;

namespace UserService.API.Middlewares;

public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException exception)
        {
            _logger.LogError($"{exception}\n\n\t");
            context.Response.StatusCode = exception.StatusCode;
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusCode = exception.StatusCode,
                Message = exception.Message
            });
        }
        catch (AlreadyExistException exception)
        {
            _logger.LogError($"{exception}\n\n\t");
            context.Response.StatusCode = exception.StatusCode;
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusCode = exception.StatusCode,
                Message = exception.Message
            });
        }
        catch (CustomException exception)
        {
            _logger.LogError($"{exception}\n\n\t");
            context.Response.StatusCode = exception.StatusCode;
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusCode = exception.StatusCode,
                Message = exception.Message
            });
        }
        catch (Exception exception)
        {
            _logger.LogError($"{exception}\n\n\t");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new Response
            {
                StatusCode = 500,
                Message = exception.Message
            });
        }
    }
}