using System.Net;

namespace Amazon.Exceptions;

public class AwsException : Exception
{
    public AwsException(string message, HttpStatusCode httpStatusCode)
        : base(message)
    {
        HttpStatusCode = httpStatusCode;
    }

    public AwsException(string message, Exception innerException, HttpStatusCode httpStatusCode)
      : base(message, innerException)
    {
        HttpStatusCode = httpStatusCode;
    }

    public HttpStatusCode HttpStatusCode { get; }
}