using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

[JsonConverter(typeof(JsonStringEnumConverter<RerankSourceType>))]
public enum RerankSourceType
{
    [JsonStringEnumMemberName("INLINE")]
    Inline = 1
}