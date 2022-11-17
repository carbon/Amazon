namespace Amazon.Security;

internal static class SigningParameterNames
{
    public const string Algorithm     = "X-Amz-Algorithm";
    public const string Credential    = "X-Amz-Credential";
    public const string Expires       = "X-Amz-Expires";
    public const string Date          = "X-Amz-Date";
    public const string SecurityToken = "X-Amz-Security-Token";
    public const string SignedHeaders = "X-Amz-SignedHeaders";
    public const string Signature     = "X-Amz-Signature";
}