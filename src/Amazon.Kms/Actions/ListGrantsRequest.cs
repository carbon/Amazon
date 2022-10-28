#nullable disable

namespace Amazon.Kms;

public sealed class ListGrantsRequest : KmsRequest
{
    public ListGrantsRequest() { }

    public ListGrantsRequest(string keyId)
    {
#if NET7_0_OR_GREATER
        ArgumentException.ThrowIfNullOrEmpty(keyId);
#else
        ArgumentNullException.ThrowIfNull(keyId);
#endif
        KeyId = keyId;
    }

    public string KeyId { get; init; }

    public int Limit { get; init; }

#nullable enable

    public string? Marker { get; init; }
}