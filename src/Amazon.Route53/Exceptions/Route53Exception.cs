using System.Net;

using Amazon.Exceptions;

namespace Amazon.Route53.Exceptions;

public sealed class Route53Exception(string message, HttpStatusCode statusCode) 
    : AwsException(message, statusCode)
{
}