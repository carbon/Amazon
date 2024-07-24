using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class Media
{
    [JsonPropertyName("MediaFileUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MediaFileUri { get; set; }

    [JsonPropertyName("RedactedMediaFileUri")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RedactedMediaFileUri { get; set; }
}
