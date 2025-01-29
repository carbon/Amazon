using System.Text.Json.Serialization;

using Amazon.Bedrock.Models;

namespace Amazon.Nova;

public sealed class NovaRequest
{
    [JsonPropertyName("schemaVersion")]
    public string SchemaVersion { get; init; } = "messages-v1";

    [JsonPropertyName("system")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ContentBlock[]? System { get; init; }

    [JsonPropertyName("messages")]
    public required NovaMessage[] Messages { get; init; }

    [JsonPropertyName("inferenceConfig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public InferenceConfig? InferenceConfig { get; init; }
}