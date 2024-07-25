using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

public sealed class GetTranscriptionJobRequest
{
    [JsonPropertyName("TranscriptionJobName")]
    public required string TranscriptionJobName { get; init; }
}