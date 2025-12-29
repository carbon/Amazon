using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

[JsonConverter(typeof(JsonStringEnumConverter<NovaEmbeddingTruncationMode>))]
public enum NovaEmbeddingTruncationMode
{
    [JsonStringEnumMemberName("NONE")]
    None = 0,

    [JsonStringEnumMemberName("START")]
    Start = 1,

    [JsonStringEnumMemberName("END")]
    End = 2
}
