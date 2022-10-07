using System.Net;

namespace InCinema.Exceptions;

public class HttpException : Exception
{
    public HttpStatusCode HttpStatusCode { get; }

    public HttpException(string? message, HttpStatusCode httpStatusCode) : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }
}