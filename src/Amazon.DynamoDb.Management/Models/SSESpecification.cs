using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models;

public sealed class SseSpecification
{
    public bool? Enabled { get; init; }

    [JsonPropertyName("KMSMasterKeyId")]
    public string? KmsMasterKeyId { get; init; }

    [JsonPropertyName("SSEType")]
    public SseType? SseType { get; init; }
}
