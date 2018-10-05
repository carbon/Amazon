namespace Amazon.Kms
{
    public sealed class AccessDeniedException : KmsException
    {
        public AccessDeniedException(string message)
            : base("AccessDeniedException", message) { }
    }
}
