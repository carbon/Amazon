using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

public sealed class SingleEmbeddingParams
{
    [JsonPropertyName("embeddingPurpose")]
    public NovaEmbeddingPurpose EmbeddingPurpose { get; set; }

    // 256 | 384 | 1024 | 3072
    [JsonPropertyName("embeddingDimension")]
    public int EmbeddingDimension { get; set; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NovaEmbeddingText? Text { get; set; }

    [JsonPropertyName("image")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NovaEmbeddingImage? Image { get; set; }

    [JsonPropertyName("audio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NovaEmbeddingAudio? Audio { get; set; }

    [JsonPropertyName("video")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public NovaEmbeddingVideo? Video { get; set; }
}
