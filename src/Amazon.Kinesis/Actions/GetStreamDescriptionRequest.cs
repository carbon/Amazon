#nullable disable

namespace Amazon.Kinesis;

public sealed class DescribeStreamRequest : KinesisRequest
{
    public DescribeStreamRequest() { }

    public DescribeStreamRequest(string streamName)
    {
        ArgumentNullException.ThrowIfNull(streamName);

        StreamName = streamName;
    }

    public string ExclusiveStartShardId { get; init; }

    public int? Limit { get; init; }

    public string StreamName { get; init; }
}

/*
{
    "ExclusiveStartShardId": "string",
    "Limit": "number",
    "StreamName": "string"
}
*/