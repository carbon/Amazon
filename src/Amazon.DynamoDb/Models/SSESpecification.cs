using System.Text.Json.Serialization;

namespace Amazon.DynamoDb.Models
{
    public sealed class SseSpecification
    {
        public bool? Enabled { get; set; }

        [JsonPropertyName("KMSMasterKeyId")]
        public string? KmsMasterKeyId { get; set; }

        [JsonPropertyName("SSEType")]
        public SseType? SseType { get; set; }
    }
}