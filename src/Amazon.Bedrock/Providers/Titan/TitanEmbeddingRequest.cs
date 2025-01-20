using System.Text.Json.Serialization;

namespace Amazon.Titan;

public sealed class TitanG1MultimodalEmbeddingRequest
{
    [JsonPropertyName("inputText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputText { get; set; }

    // PNG | JPEG
    // max = 5MB
    // max 4096 dimensions
    // max total pixels = 2048 * 2048
    // min aspect ratio 0.25, 4
    [JsonPropertyName("inputImage")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public byte[]? InputImage { get; set; }


    // [256, 384, 1024]
    [JsonPropertyName("embeddingConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TitanEmbeddingModelConfig? EmbeddingConfig { get; set; }
}

// https://docs.aws.amazon.com/bedrock/latest/userguide/titan-multiemb-models.html