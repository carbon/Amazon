using System.Net;

using Amazon.Exceptions;

namespace Amazon.Sts.Exceptions;

public sealed class StsException(string message, HttpStatusCode statusCode) 
    : AwsException(message, statusCode)
{
}