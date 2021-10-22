namespace Amazon.Kms;

public sealed class DecryptRequest : KmsRequest
{
    public DecryptRequest(
        string keyId,
        byte[] ciphertext,
        IReadOnlyDictionary<string, string>? context,
        string[]? grantTokens = null)
    {
        ArgumentNullException.ThrowIfNull(keyId);
        ArgumentNullException.ThrowIfNull(ciphertext);

        if (keyId.Length is 0)
        {
            throw new ArgumentException("May not be empty", nameof(keyId));
        }

        if (ciphertext.Length is 0)
        {
            throw new ArgumentException("May not be empty", nameof(ciphertext));
        }

        KeyId = keyId;
        CiphertextBlob = ciphertext;
        EncryptionContext = context;
        GrantTokens = grantTokens;
    }

    public string KeyId { get; }

    // [MaxSize(6144)]
    public byte[] CiphertextBlob { get; }

    public IReadOnlyDictionary<string, string>? EncryptionContext { get; }

    public string[]? GrantTokens { get; }
}