using System.Net;

namespace InCinema.Exceptions;

public class NotFoundException : HttpException
{
    public NotFoundException(string? message = null) : base(message, HttpStatusCode.NotFound) { }
}