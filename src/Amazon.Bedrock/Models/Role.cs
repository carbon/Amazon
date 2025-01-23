using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Models;

[JsonConverter(typeof(JsonStringEnumConverter<Role>))]
public enum Role
{
    [JsonStringEnumMemberName("assistant")]
    Assistant = 1,

    [JsonStringEnumMemberName("user")]
    User = 2
}