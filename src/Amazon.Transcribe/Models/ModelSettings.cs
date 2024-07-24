using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class ModelSettings
{
    [JsonPropertyName("LanguageModelName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? LanguageModelName { get; init; }
}