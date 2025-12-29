using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

public sealed class NovaEmbeddingText
{
    [JsonPropertyName("truncationMode")]
    public NovaEmbeddingTruncationMode TruncationMode { get; set; }

    // Max length: 8192 characters
    [JsonPropertyName("value")]
    public required string Value { get; set; }
}
