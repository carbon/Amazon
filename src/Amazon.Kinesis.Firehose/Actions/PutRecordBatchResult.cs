using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class PutRecordBatchResult
{
    [JsonPropertyName("FailedPutCount")]
    public int FailedPutCount { get; init; }

    [JsonPropertyName("RequestResponses")]
    public required RequestResponse[] RequestResponses { get; init; }
}