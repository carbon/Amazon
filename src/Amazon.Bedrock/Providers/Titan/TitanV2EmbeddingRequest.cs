using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Amazon.Titan;

public sealed class TitanV2EmbeddingRequest
{
    [JsonPropertyName("inputText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputText { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [DefaultValue(true)]
    public bool? Normalize { get; set; }

    // [256, 512, 1024]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [DefaultValue(1024)]
    public int? Dimensions { get; set; }
}

// https://docs.aws.amazon.com/bedrock/latest/userguide/model-parameters-titan-embed-text.html