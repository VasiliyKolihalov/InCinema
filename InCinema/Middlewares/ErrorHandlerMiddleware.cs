using System.Net;
using InCinema.Exceptions;
using Newtonsoft.Json;

namespace InCinema.Middlewares;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception error)
        {
            HttpResponse response = context.Response;
            response.ContentType = "application/json";

            response.StatusCode = error switch
            {
                NotFoundException => (int) HttpStatusCode.NotFound,
                BadRequestException => (int) HttpStatusCode.BadRequest,
                _ => (int) HttpStatusCode.InternalServerError
            };

            string result = JsonConvert.SerializeObject(new {message = error.Message});
            await response.WriteAsync(result);
        }
    }
}