using System.Diagnostics.CodeAnalysis;

namespace Amazon.Kms;

public sealed class ListGrantsRequest : KmsRequest
{
    public ListGrantsRequest() { }

    [SetsRequiredMembers]
    public ListGrantsRequest(string keyId)
    {
        ArgumentException.ThrowIfNullOrEmpty(keyId);

        KeyId = keyId;
    }

    public required string KeyId { get; init; }

    public int Limit { get; init; }

    public string? Marker { get; init; }
}