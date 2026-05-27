using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class GetVectorBucketRequest
{
    [JsonPropertyName("vectorBucketArn")]
    public string? VectorBucketArn { get; init; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; init; }
}
