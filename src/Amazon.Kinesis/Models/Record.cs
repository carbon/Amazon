using System.Text.Json.Serialization;

using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class Record : KinesisRequest, IRecord
{
    public required byte[] Data { get; set; }

    public required string PartitionKey { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExplicitHashKey { get; set; }

    [JsonPropertyName("SequenceNumber")]
    public required string SequenceNumber { get; set; }
}