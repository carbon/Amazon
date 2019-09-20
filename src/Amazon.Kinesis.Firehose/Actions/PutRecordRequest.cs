using System;

namespace Amazon.Kinesis.Firehose
{
    public sealed class PutRecordRequest
    {
        public PutRecordRequest(string deliveryStreamName, Record record)
        {
            DeliveryStreamName = deliveryStreamName ?? throw new ArgumentNullException(nameof(deliveryStreamName));
            Record = record;
        }

        public string DeliveryStreamName { get; }
        
        public Record Record { get; }
    }
}