using System.Net;

using Amazon.Exceptions;

namespace Amazon.Sts.Exceptions;

public sealed class StsException : AwsException
{
    public StsException(string message, HttpStatusCode statusCode)
        : base(message, statusCode) { }
}