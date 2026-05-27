using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public sealed class GetVectorBucketPolicyResult
{
    [JsonPropertyName("policy")]
    public string Policy { get; init; } = default!;
}
