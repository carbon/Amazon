using System.Text.Json;
using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors.Models;

public sealed class PutInputVector
{
    [JsonPropertyName("key")]
    public required string Key { get; init; }

    [JsonPropertyName("data")]
    public required VectorData Data { get; init; }

    [JsonPropertyName("metadata")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public JsonElement? Metadata { get; init; }
}