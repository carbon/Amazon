using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public sealed class GetIndexResult
{
    [JsonPropertyName("index")]
    public Amazon.S3.Vectors.Models.Index Index { get; init; } = default!;
}
