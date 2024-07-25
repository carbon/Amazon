using System.Text.Json.Serialization;

namespace Amazon.Transcribe.Serialization;

[JsonSerializable(typeof(StartTranscriptionJobRequest))]
[JsonSerializable(typeof(StartTranscriptionJobResult))]
public partial class TranscribeerializerContext : JsonSerializerContext
{
}