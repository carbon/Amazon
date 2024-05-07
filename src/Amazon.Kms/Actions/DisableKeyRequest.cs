namespace Amazon.Kms;

public sealed class DisableKeyRequest : KmsRequest
{
    public DisableKeyRequest(string keyId)
    {
        ArgumentException.ThrowIfNullOrEmpty(keyId);

        KeyId = keyId;
    }

    public required string KeyId { get; init; }
}