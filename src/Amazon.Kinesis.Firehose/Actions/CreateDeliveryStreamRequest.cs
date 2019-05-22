#nullable disable

namespace Amazon.Kinesis.Firehose
{
    public class CreateDeliveryStreamRequest
    {
        public string DeliveryStreamName { get; set; }

        public DeliveryStreamType DeliveryStreamType { get; set; }

        public ExtendedS3DestinationConfiguration ExtendedS3DestinationConfiguration { get; set; }
    }
}