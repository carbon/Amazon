using System.Formats.Asn1;

namespace Amazon.Kms;

public sealed class CancelKeyDeletionRequest(string keyId) : KmsRequest
{
    public string KeyId { get; } = keyId ?? throw new ArgumentNullException(nameof(keyId));
}