using System.Net;
using BookShop.Auth.DTOAuth.Responses;
using FluentValidation;
using Newtonsoft.Json;

namespace BookShop.Auth.SharedAuth;

public class GlobalExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex) // Обработка ошибок валидации
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errors = ex.Errors.Select(x => x.ErrorMessage); // Извлекаем сообщения об ошибках

            var errorRes = Result<IEnumerable<string>>.Error(errors, "Validation error");

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(errorRes));
        }
        catch (Exception ex) // Обработка других ошибок
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var errorRes = Result<string>.Error(exception.Message, "Internal server error");

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(errorRes));
    }
}