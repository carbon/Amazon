using System;
using System.Runtime.Serialization;

namespace Amazon.Kinesis.Firehose
{
    public class PutRecordBatchRequest
    {
        public PutRecordBatchRequest() { }

        public PutRecordBatchRequest(string deliveryStreamName, params Record[] records)
        {
            DeliveryStreamName = deliveryStreamName ?? throw new ArgumentNullException(nameof(deliveryStreamName));
            Records = records;
        }

        [DataMember(Name = "deliveryStreamName")]
        public string DeliveryStreamName { get; set; }

        [DataMember(Name = "records")]
        public Record[] Records { get; set; }
    }
}
