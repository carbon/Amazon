namespace Amazon.Kinesis;

public sealed class StreamDescription
{
    public required bool HasMoreShards { get; init; }

    public required int RetentionPeriodHours { get; set; }

    public required List<Shard> Shards { get; init; }

    public required string StreamARN { get; init; }

    public required  string StreamName { get; init; }

    public required string StreamStatus { get; init; }

    public string? KeyId { get; init; }
}