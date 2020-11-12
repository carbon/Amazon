using System;

using Amazon.Scheduling;

namespace Amazon.Exceptions
{
    public sealed class ServiceUnavailableException : Exception, IException
    {
        public ServiceUnavailableException()
            : base("Service unavailable") { }

        public bool IsTransient => true;
    }
}