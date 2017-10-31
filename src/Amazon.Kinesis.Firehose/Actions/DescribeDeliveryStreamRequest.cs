using System.Runtime.Serialization;

namespace Amazon.Kinesis.Firehose
{
    public class DescribeDeliveryStreamRequest
    {
        public string DeliveryStreamName { get; set; }

        public string ExclusiveStartDestinationId { get; set; }

        [DataMember(Name = "limit", EmitDefaultValue = false)]
        public int? Limit { get; set; }
    }
}
