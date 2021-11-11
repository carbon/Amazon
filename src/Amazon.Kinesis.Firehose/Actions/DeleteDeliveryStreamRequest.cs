using System;

namespace Amazon.Kinesis.Firehose;

public sealed class DeleteDeliveryStreamRequest
{
    public DeleteDeliveryStreamRequest(string deliveryStreamName)
    {
        ArgumentNullException.ThrowIfNull(deliveryStreamName);

        DeliveryStreamName = deliveryStreamName;
    }

    public bool? AllowForceDelete { get; init; }

    public string DeliveryStreamName { get; init; }
}
