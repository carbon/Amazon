using System.Text.Json.Serialization;

namespace Cohere;

// NONE|START|END
[JsonConverter(typeof(JsonStringEnumConverter<Truncate>))]
public enum Truncate
{
    /// <summary>
    /// NONE – (Default) Returns an error when the input exceeds the maximum input token length.
    /// </summary>
    [JsonStringEnumMemberName("NONE")]
    NONE = 0,

    [JsonStringEnumMemberName("START")]
    START = 1,

    [JsonStringEnumMemberName("END")]
    END = 2
}
