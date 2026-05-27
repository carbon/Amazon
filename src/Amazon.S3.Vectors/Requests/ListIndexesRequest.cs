using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class ListIndexesRequest
{
    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; set; }

    [JsonPropertyName("vectorBucketArn")]
    public string? VectorBucketArn { get; set; }

    [JsonPropertyName("prefix")]
    public string? Prefix { get; set; }

    [JsonPropertyName("maxResults")]
    public int? MaxResults { get; set; }

    [JsonPropertyName("nextToken")]
    public string? NextToken { get; set; }
}
