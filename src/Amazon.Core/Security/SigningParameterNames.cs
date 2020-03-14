namespace Amazon.Security
{
    internal static class SigningParameterNames
    {
        public static readonly string Algorithm     = "X-Amz-Algorithm";
        public static readonly string Credential    = "X-Amz-Credential";
        public static readonly string Expires       = "X-Amz-Expires";
        public static readonly string Date          = "X-Amz-Date";
        public static readonly string SecurityToken = "X-Amz-Security-Token";
        public static readonly string SignedHeaders = "X-Amz-SignedHeaders";
        public static readonly string Signature     = "X-Amz-Signature";
    }
}