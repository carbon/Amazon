using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class CreateDeliveryStreamRequest
{
    [JsonPropertyName("DeliveryStreamName")]
    public required string DeliveryStreamName { get; init; }

    [JsonPropertyName("DeliveryStreamType")]
    public DeliveryStreamType DeliveryStreamType { get; init; }

    [JsonPropertyName("ExtendedS3DestinationConfiguration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public ExtendedS3DestinationConfiguration? ExtendedS3DestinationConfiguration { get; init; }
}