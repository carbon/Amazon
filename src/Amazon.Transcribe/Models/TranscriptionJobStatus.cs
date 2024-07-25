using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

[JsonConverter(typeof(JsonStringEnumConverter<TranscriptionJobStatus>))]
public enum TranscriptionJobStatus
{
    Unknown = 0,
    QUEUED = 1,
    IN_PROGRESS = 2,
    FAILED = 3,
    COMPLETED = 4
}