using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class Transcript
{
    [JsonPropertyName("RedactedTranscriptFileUri")]
    public string? RedactedTranscriptFileUri { get; set; }

    [JsonPropertyName("TranscriptFileUri")]
    public string? TranscriptFileUri { get; set; }
}