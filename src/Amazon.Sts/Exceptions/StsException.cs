using System;
using System.Net;

namespace Amazon.Sts.Exceptions;

public sealed class StsException : Exception
{
    public StsException(string message)
        : base(message) { }


    public StsException(HttpStatusCode statusCode, string message)
        : base(message)
    {
        StatusCode = statusCode;
    }


    public HttpStatusCode StatusCode { get; }
}