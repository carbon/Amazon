using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models;

public sealed class SseDescription
{
    public Timestamp InaccessibleEncryptionDateTime { get; set; }

    [JsonPropertyName("KMSMasterKeyArn")]
    public string? KmsMasterKeyArn { get; set; }

    [JsonPropertyName("SSEType")]
    public SseType? SseType { get; set; }

    public string? Status { get; set; }
}
