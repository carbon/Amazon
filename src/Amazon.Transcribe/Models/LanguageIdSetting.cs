using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class LanguageIdSetting
{
    [JsonPropertyName("LanguageModelName")]
    [StringLength(200)]
    public string? LanguageModelName { get; init; }

    [JsonPropertyName("VocabularyFilterName")]
    public string? VocabularyFilterName { get; init; }

    [JsonPropertyName("VocabularyName")]
    public string? VocabularyName { get; init; }
}