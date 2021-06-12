using System;

namespace Amazon.Kinesis.Firehose
{
    public sealed class DeleteDeliveryStreamRequest
    {
        public DeleteDeliveryStreamRequest(string deliveryStreamName)
        {
            DeliveryStreamName = deliveryStreamName ?? throw new ArgumentNullException(nameof(deliveryStreamName));
        }

        public bool? AllowForceDelete { get; init; }

        public string DeliveryStreamName { get; init; }
    }
}
