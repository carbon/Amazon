using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Actions;

[JsonConverter(typeof(JsonStringEnumConverter<RerankDocumentType>))]
public enum RerankDocumentType
{
    [JsonStringEnumMemberName("TEXT")]
    Text = 1,

    [JsonStringEnumMemberName("JSON")]
    Json = 2
}