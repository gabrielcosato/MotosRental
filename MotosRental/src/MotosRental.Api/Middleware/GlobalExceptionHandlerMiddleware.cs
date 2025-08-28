using System.Net;
using System.Text.Json;
using MotosRental.Exceptions;

namespace MotosRental.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        _logger.LogError(exception, "Ocorreu um erro inesperado: {Message}", exception.Message);

        HttpStatusCode statusCode;
        string message;

        switch (exception)
        {
            
            case DuplicateLicensePlateException ex:
                statusCode = HttpStatusCode.Conflict;
                message = ex.Message;
                break;
            case InvalidLicensePlateException ex:
                statusCode = HttpStatusCode.BadRequest;
                message = ex.Message;
                break;
            case KeyNotFoundException ex: 
                statusCode = HttpStatusCode.NotFound; 
                message = ex.Message;
                break;
            case DuplicateDataException ex:
                statusCode = HttpStatusCode.Conflict; 
                message = ex.Message;
                break;
            case BusinessValidationException ex:
                statusCode = HttpStatusCode.BadRequest; 
                message = ex.Message;
                break;
            case Microsoft.EntityFrameworkCore.DbUpdateException ex when ex.InnerException is Npgsql.PostgresException pgEx && pgEx.SqlState == "23505":
                statusCode = HttpStatusCode.Conflict; 
                message = "Já existe um registro com os dados informados (CNPJ, Email ou CNH).";
                break;
            default:
                statusCode = HttpStatusCode.InternalServerError; 
                message = "Ocorreu um erro interno no servidor. Tente novamente mais tarde.";
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new { error = message };
        await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
