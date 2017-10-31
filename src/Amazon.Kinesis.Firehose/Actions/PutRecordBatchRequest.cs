using System;

namespace Amazon.Kinesis.Firehose
{
    public class PutRecordBatchRequest
    {
        public PutRecordBatchRequest(string deliveryStreamName, params Record[] records)
        {
            DeliveryStreamName = deliveryStreamName ?? throw new ArgumentNullException(nameof(deliveryStreamName));
            Records            = records            ?? throw new ArgumentNullException(nameof(records));
        }

        public string DeliveryStreamName { get; }

        public Record[] Records { get; }
    }
}
