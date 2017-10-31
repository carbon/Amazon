namespace Amazon.Kinesis.Firehose
{
    public class ListDeliveryStreamsRequest
    {
        public string DeliveryStreamType { get; set; }

        public string ExclusiveStartDeliveryStreamName { get; set; }

        public int? Limit { get; set; }
    }


    public class ListDeliveryStreamsResult
    {
    }
}