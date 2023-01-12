using System.Diagnostics.CodeAnalysis;

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

    public string? ExclusiveStartShardId { get; init; }

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