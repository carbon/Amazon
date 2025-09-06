using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class DeleteDeliveryStreamRequest
{
    public DeleteDeliveryStreamRequest(string deliveryStreamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(deliveryStreamName);

        DeliveryStreamName = deliveryStreamName;
    }

    [JsonPropertyName("AllowForceDelete")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? AllowForceDelete { get; init; }

    [JsonPropertyName("DeliveryStreamName")]
    public required string DeliveryStreamName { get; init; }
}