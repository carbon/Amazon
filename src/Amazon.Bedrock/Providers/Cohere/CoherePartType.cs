using System.Text.Json.Serialization;

namespace Cohere;

[JsonConverter(typeof(JsonStringEnumConverter<CoherePartType>))]
public enum CoherePartType
{
    [JsonStringEnumMemberName("text")] 
    Text,

    [JsonStringEnumMemberName("image_url")] 
    ImageUrl
}
