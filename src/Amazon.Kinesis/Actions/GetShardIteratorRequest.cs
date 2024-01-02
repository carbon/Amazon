using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

public class GetShardIteratorRequest : KinesisRequest
{
    public GetShardIteratorRequest(
        string streamName,
        string shardId,
        ShardIteratorType type,
        string? startingSequenceNumber = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(streamName);
        ArgumentException.ThrowIfNullOrEmpty(shardId);

        StreamName = streamName;
        ShardId = shardId;
        ShardIteratorType = type;
        StartingSequenceNumber = startingSequenceNumber;
    }

    public string ShardId { get; }

    public ShardIteratorType ShardIteratorType { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StartingSequenceNumber { get; }

    public string StreamName { get; }
}