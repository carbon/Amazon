using System.Text.Json.Serialization;

namespace Amazon.S3.Vectors;

public class ListVectorsRequest
{
    [JsonPropertyName("indexArn")]
    public string? IndexArn { get; set; }

    [JsonPropertyName("indexName")]
    public string? IndexName { get; set; }

    [JsonPropertyName("vectorBucketName")]
    public string? VectorBucketName { get; set; }

    [JsonPropertyName("maxResults")]
    public int? MaxResults { get; set; }

    [JsonPropertyName("nextToken")]
    public string? NextToken { get; set; }

    [JsonPropertyName("segmentCount")]
    public int? SegmentCount { get; set; }

    [JsonPropertyName("segmentIndex")]
    public int? SegmentIndex { get; set; }

    [JsonPropertyName("returnData")]
    public bool? ReturnData { get; set; }

    [JsonPropertyName("returnMetadata")]
    public bool? ReturnMetadata { get; set; }
}
