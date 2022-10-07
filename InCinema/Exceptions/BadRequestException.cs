using System.Net;

namespace InCinema.Exceptions;

public class BadRequestException : HttpException
{
    public BadRequestException(string? message = null) : base(message, HttpStatusCode.BadRequest) { }
}