namespace Amazon.Kms;

public sealed class CreateAliasRequest : KmsRequest
{
    public CreateAliasRequest(string targetKeyId, string aliasName)
    {
#if NET7_0_OR_GREATER
        ArgumentException.ThrowIfNullOrEmpty(targetKeyId);
        ArgumentException.ThrowIfNullOrEmpty(aliasName);
#else
        ArgumentNullException.ThrowIfNull(targetKeyId);
        ArgumentNullException.ThrowIfNull(aliasName);
#endif
        TargetKeyId = targetKeyId;
        AliasName = aliasName;
    }

    public string TargetKeyId { get; }

    public string AliasName { get; }
}    