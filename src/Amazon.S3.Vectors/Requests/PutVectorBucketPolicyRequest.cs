using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class PutVectorBucketPolicyRequest
{
    [JsonPropertyName("vectorBucketArn")]
    public string? VectorBucketArn { get; set; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; set; }

    [JsonPropertyName("policy")]
    public string Policy { get; set; } = default!;
}
