using System.Net;

namespace InCinema.Exceptions;

public class ForbiddenException : HttpException
{
    public ForbiddenException(string? message = null) : base(message, HttpStatusCode.Forbidden) { }
}