using System.Text.Json.Serialization;

namespace Amazon.Transcribe;

[JsonConverter(typeof(JsonStringEnumConverter<RedactionType>))]
public enum RedactionType
{
    PII = 1
}
