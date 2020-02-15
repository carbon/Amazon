#nullable disable

namespace Amazon.Kinesis.Firehose
{
    public sealed class DescribeDeliveryStreamRequest
    {
        public string DeliveryStreamName { get; set; }

        public string ExclusiveStartDestinationId { get; set; }

        public int? Limit { get; set; }
    }
}