using System.Text.Json.Serialization;

namespace Amazon.Bedrock.Nova;

public sealed class NovaEmbeddingRequest
{
    // nova-multimodal-embed-v1
    [JsonPropertyName("schemaVersion")]
    public required string SchemaVersion { get; init; }

    [JsonPropertyName("taskType")]
    public required NovaEmbeddingTaskType TaskType { get; init; }

    [JsonPropertyName("singleEmbeddingParams")]
    public required SingleEmbeddingParams SingleEmbeddingParams { get; init; }
}