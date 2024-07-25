using System.Text.Json.Serialization;

namespace Amazon.Transcribe.Serialization;

[JsonSerializable(typeof(GetTranscriptionJobRequest))]
[JsonSerializable(typeof(GetTranscriptionJobResult))]
[JsonSerializable(typeof(StartTranscriptionJobRequest))]
[JsonSerializable(typeof(StartTranscriptionJobResult))]
public partial class TranscribeSerializerContext : JsonSerializerContext
{
}