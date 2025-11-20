using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;


public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //  here we log or  request and the responses as well
            _logger.LogInformation("the reuqest information is"+context.Request.Body,context.Request.Headers);
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, _logger);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger logger)
    {
        logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
        // logger.LogInformation(context.Response.StatusCode+"");

        var statusCode = ex switch
        {
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError,
        };

        var problem = new ProblemDetails
        {
            Status =statusCode,
            Title = ex.GetType().Name,
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = statusCode;
        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        await context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
    }
    //  useMiddlewares
    // public static class GlobalExceptionHandler
    // {
    //     public static IApplicationBuilder UseGlobalExceptionHandler( this IApplicationBuilder build)
    //     {
    //         return build.UseMiddleware<GlobalExceptionMiddleware>();
    //     }
    // }
    // This class should be static

}
public static class GlobalExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder build)
    {
        return build.UseMiddleware<GlobalExceptionMiddleware>();
    }
}

