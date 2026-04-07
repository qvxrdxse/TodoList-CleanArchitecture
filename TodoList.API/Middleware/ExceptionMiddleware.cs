using System.Net;
using System.Text.Json;
using TodoList.Application.Exceptions;

namespace TodoList.API.Middleware;
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static Task HandleException(HttpContext context, Exception ex)
    {
        var statusCode = HttpStatusCode.InternalServerError;

        switch (ex)
        {
            case NotFoundException:
                statusCode = HttpStatusCode.NotFound;
                break;
        }

        var response = new
        {
            error = ex.Message
        };

        var json = JsonSerializer.Serialize(response);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        return context.Response.WriteAsync(json);
    }
}