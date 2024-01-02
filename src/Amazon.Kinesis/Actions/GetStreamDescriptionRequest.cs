using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

public sealed class DescribeStreamRequest : KinesisRequest
{
    public DescribeStreamRequest() { }

    [SetsRequiredMembers]
    public DescribeStreamRequest(string streamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(streamName);

        StreamName = streamName;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExclusiveStartShardId { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Limit { get; init; }

    public required string StreamName { get; init; }
}

/*
{
    "ExclusiveStartShardId": "string",
    "Limit": "number",
    "StreamName": "string"
}
*/