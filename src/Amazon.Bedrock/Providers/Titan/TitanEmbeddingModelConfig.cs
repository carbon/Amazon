using System.Text.Json.Serialization;

namespace Amazon.Titan;

public sealed class TitanEmbeddingModelConfig
{
    // [256, 384, 1024]
    [JsonPropertyName("OutputEmbeddingLength")]
    public int OutputEmbeddingLength { get; set; }
}

// https://docs.aws.amazon.com/bedrock/latest/userguide/model-parameters-titan-text.html