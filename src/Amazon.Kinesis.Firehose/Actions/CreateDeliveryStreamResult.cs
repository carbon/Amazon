using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class CreateDeliveryStreamResult
{
    [JsonPropertyName("DeliveryStreamARN")]
    public required string DeliveryStreamARN { get; init; }
}