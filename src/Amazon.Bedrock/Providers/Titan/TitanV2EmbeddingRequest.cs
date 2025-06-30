using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Amazon.Titan;

public sealed class TitanV2EmbeddingRequest
{
    [JsonPropertyName("inputText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputText { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [DefaultValue(true)]
    public bool? Normalize { get; init; }

    // 256 | 512 | 1024
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [DefaultValue(1024)]
    public int? Dimensions { get; init; }
}

// https://docs.aws.amazon.com/bedrock/latest/userguide/model-parameters-titan-embed-text.html