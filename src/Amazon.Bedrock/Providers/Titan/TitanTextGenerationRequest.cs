using System.Text.Json.Serialization;

namespace Amazon.Titan;

public sealed class TitanTextGenerationRequest
{
    [JsonPropertyName("inputText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? InputText { get; set; }

    [JsonPropertyName("textGenerationConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public TextGenerationConfiguration? TextGenerationConfig { get; set; }
}

// https://docs.aws.amazon.com/bedrock/latest/userguide/model-parameters-titan-text.html