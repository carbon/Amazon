using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public class Subtitles
{
    // vtt | srt
    [JsonPropertyName("Formats")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? Formats { get; init; }

    [JsonPropertyName("OutputStartIndex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int OutputStartIndex { get; init; } = 0;

    [JsonPropertyName("SubtitleFileUris")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public List<string>? SubtitleFileUris { get; init; }
}