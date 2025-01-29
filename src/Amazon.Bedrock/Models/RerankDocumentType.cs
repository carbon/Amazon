using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

[JsonConverter(typeof(JsonStringEnumConverter<RerankDocumentType>))]
public enum RerankDocumentType
{
    [JsonStringEnumMemberName("TEXT")]
    Text = 1,

    [JsonStringEnumMemberName("JSON")]
    Json = 2
}
