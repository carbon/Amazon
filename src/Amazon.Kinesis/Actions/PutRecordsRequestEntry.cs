using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Actions;

public sealed class PutRecordsRequestEntry
{
    public required byte[] Data { get; init; }

    public required byte[] PartitionKey { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExplicitHashKey { get; init; }
}