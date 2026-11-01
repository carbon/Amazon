using System.Text.Json.Serialization;

namespace Cohere;

[JsonConverter(typeof(JsonStringEnumConverter<Cohere4Truncate>))]
public enum Cohere4Truncate
{
    /// <summary>
    /// NONE – (Default) Returns an error when the input exceeds the maximum input token length.
    /// </summary>
    [JsonStringEnumMemberName("NONE")]
    None = 0,

    [JsonStringEnumMemberName("LEFT")]
    Left = 1,

    [JsonStringEnumMemberName("RIGHT")]
    Right = 2
}