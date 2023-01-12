namespace Amazon.Kinesis.Firehose;

public sealed class DeleteDeliveryStreamRequest
{
    public DeleteDeliveryStreamRequest(string deliveryStreamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(deliveryStreamName);

        DeliveryStreamName = deliveryStreamName;
    }

    public bool? AllowForceDelete { get; init; }

    public required string DeliveryStreamName { get; init; }
}