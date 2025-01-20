using System.Text.Json.Serialization;

namespace Amazon.Titan;

public sealed class TitanEmbeddingResult
{
    [JsonPropertyName("embedding")]
    public required float[] Embedding { get; init; }

    [JsonPropertyName("inputTextTokenCount")]
    public required int InputTextTokenCount { get; init; }
}