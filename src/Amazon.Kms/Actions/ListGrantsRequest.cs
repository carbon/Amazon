#nullable disable

namespace Amazon.Kms;

public sealed class ListGrantsRequest : KmsRequest
{
    public ListGrantsRequest() { }

    public ListGrantsRequest(string keyId)
    {
        ArgumentNullException.ThrowIfNull(keyId);

        KeyId = keyId;
    }

    public string KeyId { get; init; }

    public int Limit { get; init; }

#nullable enable

    public string? Marker { get; init; }
}