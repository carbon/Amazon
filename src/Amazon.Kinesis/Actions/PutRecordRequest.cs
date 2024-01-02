using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

public sealed class PutRecordRequest : KinesisRequest
{
    public PutRecordRequest(byte[] data)
    {
        if (data.Length > 1_048_576)
        {
            throw new ArgumentException("Must be 1,048,576 or fewer bytes", nameof(data));
        }

        Data = data;
    }

    public byte[] Data { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExplicitHashKey { get; set; }

    [JsonPropertyName("PartitionKey")]
    public required string PartitionKey { get; set; }

    [JsonPropertyName("SequenceNumberForOrdering")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SequenceNumberForOrdering { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StreamARN { get; init; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? StreamName { get; init; }
}