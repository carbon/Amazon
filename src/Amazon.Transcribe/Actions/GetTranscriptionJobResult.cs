using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class GetTranscriptionJobResult
{
    [JsonPropertyName("TranscriptionJob")]
    public required TranscriptionJob TranscriptionJob { get; init; }
}
