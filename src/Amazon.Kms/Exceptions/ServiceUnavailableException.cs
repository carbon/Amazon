using Amazon.Scheduling;

namespace Amazon.Kms
{
    public sealed class ServiceUnavailableException : KmsException, IException
    {
        public ServiceUnavailableException(string message)
            : base("ServiceUnavailableException", message) { }

        public bool IsTransient => true;
    }
}
