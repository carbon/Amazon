using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class DecryptRequest : KmsRequest
{
    public DecryptRequest(
        string keyId,
        byte[] ciphertext,
        IReadOnlyDictionary<string, string>? context,
        string[]? grantTokens = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(keyId);
        ArgumentNullException.ThrowIfNull(ciphertext);

        if (ciphertext.Length is 0)
        {
            throw new ArgumentException("Must not be empty", nameof(ciphertext));
        }

        if (ciphertext.Length > 6_144)
        {
            throw new ArgumentException($"Must be 6,144 or fewer bytes. Was {ciphertext.Length} bytes.", nameof(ciphertext));
        }

        KeyId = keyId;
        CiphertextBlob = ciphertext;
        EncryptionContext = context;
        GrantTokens = grantTokens;
    }

    public string KeyId { get; }

    public byte[] CiphertextBlob { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public EncryptionAlgorithm? EncryptionAlgorithm { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IReadOnlyDictionary<string, string>? EncryptionContext { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? GrantTokens { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public RecipientInfo[]? Recipient { get; }
}