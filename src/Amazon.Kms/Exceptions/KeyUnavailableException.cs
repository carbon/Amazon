using Amazon.Scheduling;

namespace Amazon.Kms
{
    public sealed class KeyUnavailableException : KmsException, IException
    {
        public KeyUnavailableException(string message)
            : base("KeyUnavailableException", message) { }


        public bool IsTransient => true;
    }
}
