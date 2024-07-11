using System.Net;

using Amazon.Exceptions;

namespace Amazon.Elb.Exceptions;

public sealed class ElbException(Error error, HttpStatusCode statusCode)
    : AwsException(error.Message, statusCode)
{
    public Error Error { get; } = error;
}