using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? Limit { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? Marker { get; init; }
}