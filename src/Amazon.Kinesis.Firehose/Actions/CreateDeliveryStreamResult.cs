using System.Runtime.Serialization;

namespace Amazon.Kinesis.Firehose
{
    public class CreateDeliveryStreamResult
    {
        [DataMember(Name = "DeliveryStreamARN")]
        public string DeliveryStreamARN { get; set; }
    }
}