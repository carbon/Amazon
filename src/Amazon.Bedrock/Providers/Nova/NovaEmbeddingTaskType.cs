using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

[JsonConverter(typeof(JsonStringEnumConverter<NovaEmbeddingTaskType>))]
public enum NovaEmbeddingTaskType
{
    [JsonStringEnumMemberName("SINGLE_EMBEDDING")]
    SingleEmbedding = 1
}