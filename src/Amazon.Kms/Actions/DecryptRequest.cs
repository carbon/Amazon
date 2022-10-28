namespace Amazon.Kms;

public sealed class DecryptRequest : KmsRequest
{
    public DecryptRequest(
        string keyId,
        byte[] ciphertext,
        IReadOnlyDictionary<string, string>? context,
        string[]? grantTokens = null)
    {
#if NET7_0_OR_GREATER
        ArgumentException.ThrowIfNullOrEmpty(keyId);
#else
        ArgumentNullException.ThrowIfNull(keyId);

        if (keyId.Length is 0)
        {
            throw new ArgumentException("Must not be empty", nameof(keyId));
        }
#endif

        ArgumentNullException.ThrowIfNull(ciphertext);

        if (ciphertext.Length is 0)
        {
            throw new ArgumentException("Must not be empty", nameof(ciphertext));
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