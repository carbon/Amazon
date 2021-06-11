using Amazon.Scheduling;

namespace Amazon.Kms
{
    public sealed class KeyUnavailableException : KmsException, IException
    {
        public KeyUnavailableException(KmsError error)
            : base(error) { }

        public bool IsTransient => true;
    }
}