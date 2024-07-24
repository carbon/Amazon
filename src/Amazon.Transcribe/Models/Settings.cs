using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class Settings
{
    [JsonPropertyName("ChannelIdentification")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ChannelIdentification { get; init; }

    [JsonPropertyName("MaxAlternatives")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxAlternatives { get; init; }

    [JsonPropertyName("MaxSpeakerLabels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxSpeakerLabels { get; init; }

    [JsonPropertyName("ShowAlternatives")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ShowAlternatives { get; init; }

    [JsonPropertyName("ShowSpeakerLabels")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? ShowSpeakerLabels { get; init; }

    // remove | mask | tag
    [JsonPropertyName("VocabularyFilterMethod")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? VocabularyFilterMethod { get; init; }

    [JsonPropertyName("VocabularyFilterName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? VocabularyFilterName { get; init; }

    [JsonPropertyName("VocabularyName")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? VocabularyName { get; init; }
}