namespace Amazon.Kms;

public sealed class EnableKeyRequest : KmsRequest
{
    public EnableKeyRequest(string keyId)
    {
        ArgumentException.ThrowIfNullOrEmpty(keyId);

        KeyId = keyId;
    }

    public string KeyId { get; }
}