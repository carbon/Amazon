using Amazon.Scheduling;

namespace Amazon.Kms
{
    public sealed class ServiceUnavailableException : KmsException, IException
    {
        public ServiceUnavailableException(KmsError error)
            : base(error.Message, error.Type) { }

        public bool IsTransient => true;
    }
}
