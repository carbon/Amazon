using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Nova;

public sealed class InferenceConfig
{
    /// <summary>
    // Amount of randomness injected into the response.
    // Defaults to 1.0. Ranges from 0.0 to 1.0.
    // Use temperature closer to 0.0 for analytical / multiple choice, and closer to 1.0 for creative and generative tasks.
    /// </summary>
    [JsonPropertyName("temperature")]
    [Range(0, 1)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? Temperature { get; init; }

    /// <summary>
    /// Top K (topK) – Specify the number of token choices the model uses to generate the next token.
    /// </summary>
    [JsonPropertyName("top_k")]
    [DefaultValue(250)]
    [Range(0, 500)]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? TopK { get; init; }

    /// <summary>
    /// Use a lower value to ignore less probable options.
    /// </summary>
    [JsonPropertyName("top_p")]
    [Range(0, 1)] // default = 1
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public float? TopP { get; init; }

    [JsonPropertyName("max_new_tokens")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxNewTokens { get; init; }
}