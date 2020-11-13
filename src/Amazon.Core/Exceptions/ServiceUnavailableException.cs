using System;
using System.Net;

using Amazon.Scheduling;

namespace Amazon.Exceptions
{
    public sealed class ServiceUnavailableException : AwsException, IException
    {
        public ServiceUnavailableException()
            : base("Service unavailable", HttpStatusCode.ServiceUnavailable) { }

        public bool IsTransient => true;
    }
}