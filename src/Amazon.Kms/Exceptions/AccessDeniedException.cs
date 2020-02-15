using Amazon.Scheduling;

namespace Amazon.Kms
{
    public sealed class AccessDeniedException : KmsException, IException
    {
        public AccessDeniedException(string message)
            : base("AccessDeniedException", message) { }

        public bool IsTransient => false;
    }
}