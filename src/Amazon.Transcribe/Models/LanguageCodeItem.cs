using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public readonly struct LanguageCodeItem
{
    /// <summary>
    /// Provides the total time, in seconds, each identified language is spoken in your media.
    /// </summary>
    [JsonPropertyName("DurationInSeconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public double DurationInSeconds { get; init; }

    [JsonPropertyName("LanguageCode")]
    public string LanguageCodeValue { get; init; }
}
