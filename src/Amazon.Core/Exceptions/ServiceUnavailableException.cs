using System.Net;

using Amazon.Scheduling;

namespace Amazon.Exceptions;

public sealed class ServiceUnavailableException : AwsException, IException
{
    public ServiceUnavailableException()
        : base("Service unavailable", HttpStatusCode.ServiceUnavailable) { }

    public ServiceUnavailableException(string message)
        : base(message, HttpStatusCode.ServiceUnavailable) { }

    public bool IsTransient => true;
}