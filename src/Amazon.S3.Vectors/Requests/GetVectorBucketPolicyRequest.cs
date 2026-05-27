using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class GetVectorBucketPolicyRequest
{
    [JsonPropertyName("vectorBucketArn")]
    public string? VectorBucketArn { get; set; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; set; }
}
