﻿namespace InCinema.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException(string? message = null) : base(message) { }
}