namespace Amazon.Kms;

public sealed class CreateAliasRequest : KmsRequest
{
    public CreateAliasRequest(string targetKeyId, string aliasName)
    {
        ArgumentException.ThrowIfNullOrEmpty(targetKeyId);
        Ensure.LengthBetween(aliasName, 1, 256);

        TargetKeyId = targetKeyId;
        AliasName = aliasName;
    }

    public string TargetKeyId { get; }

    public string AliasName { get; }
}
