using System.Text.Json.Serialization;

namespace Amazon.Kms;

public sealed class EncryptRequest : KmsRequest
{
    public EncryptRequest(
        string keyId,
        byte[] plaintext,
        IReadOnlyDictionary<string, string>? context = null,
        string[]? grantTokens = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(keyId);
        ArgumentNullException.ThrowIfNull(plaintext);

        if (keyId.Length is 0)
        {
            throw new ArgumentException("Must not be empty", nameof(keyId));
        }

        if (plaintext.Length > 1024 * 4)
        {
            throw new ArgumentException("Must be less than 4KB", nameof(plaintext));
        }

        KeyId = keyId;
        Plaintext = plaintext;
        EncryptionContext = context;
        GrantTokens = grantTokens;
    }

    public EncryptionAlgorithm? EncryptionAlgorithm { get; init; }

    /// <summary>
    /// An encryption context is a key/value pair that you can pass to 
    /// AWS KMS when you call the Encrypt function.
    /// It is integrity checked but not stored as part of the ciphertext 
    /// that is returned. Although the encryption context is not literally 
    /// included in the ciphertext, it is cryptographically bound to the 
    /// ciphertext during encryption and must be passed again when you 
    /// call the Decrypt function. 
    /// Decryption will only succeed if the value you pass for decryption
    /// is exactly the same as the value you passed during encryption
    /// and the ciphertext has not been tampered with. 
    /// </summary>
    public IReadOnlyDictionary<string, string>? EncryptionContext { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? GrantTokens { get; }

    public string KeyId { get; }

    // EncryptionAlgorithm

    public byte[] Plaintext { get; }
}
