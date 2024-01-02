using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

using Carbon.Data.Streams;

namespace Amazon.Kinesis;

public sealed class Record : KinesisRequest, IRecord
{
    public Record() { }

    [SetsRequiredMembers]
    public Record(string streamName, byte[] data)
    {
        if (data.Length > 1048576)
        {
            throw new ArgumentException("Must be 1048576 or fewer bytes", nameof(data));
        }

        StreamName = streamName;
        Data = data;
    }

    public required byte[] Data { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExplicitHashKey { get; set; }

    public string? PartitionKey { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SequenceNumberForOrdering { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StreamName { get; init; }
}