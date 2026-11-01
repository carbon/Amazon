using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

[JsonConverter(typeof(JsonStringEnumConverter<OutputFormatType>))]
public enum OutputFormatType
{
    [JsonStringEnumMemberName("json_schema")]
    JsonSchema
}
