using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class StartTranscriptionJobResult
{
    [JsonPropertyName("TranscriptionJob")]
    public required TranscriptionJob TranscriptionJob { get; set; }
}