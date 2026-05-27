using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public sealed class CreateVectorBucketResult
{
    [JsonPropertyName("vectorBucketArn")]
    public string VectorBucketArn { get; init; } = null!;
}