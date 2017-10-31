using System;

namespace Amazon.Kinesis.Firehose
{
    public class DeleteDeliveryStreamRequest
    {
        public DeleteDeliveryStreamRequest(string deliveryStreamName)
        {
            DeliveryStreamName = deliveryStreamName ?? throw new ArgumentNullException(nameof(deliveryStreamName));
        }

        public string DeliveryStreamName { get; set; }
    }
}
