using System;
using System.Runtime.Serialization;

namespace Amazon.Kinesis.Firehose
{
    public class PutRecordRequest
    {
        public PutRecordRequest() { }

        public PutRecordRequest(string deliveryStreamName, Record record)
        {
            DeliveryStreamName = deliveryStreamName ?? throw new ArgumentNullException(nameof(deliveryStreamName));
            Record = record;
        }

        [DataMember(Name = "deliveryStreamName")]
        public string DeliveryStreamName { get; set; }
        
        [DataMember(Name = "record")]
        public Record Record { get; set; }
    }
}