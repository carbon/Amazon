using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class DescribeDeliveryStreamRequest
{
    [JsonPropertyName("DeliveryStreamName")]
    public required string DeliveryStreamName { get; init; }

    [JsonPropertyName("ExclusiveStartDestinationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ExclusiveStartDestinationId { get; init; }

    [JsonPropertyName("Limit")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Limit { get; init; }
}