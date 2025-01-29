using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

[JsonConverter(typeof(JsonStringEnumConverter<RerankQueryType>))]
public enum RerankQueryType
{
    [JsonStringEnumMemberName("TEXT")]
    Text = 1
}
