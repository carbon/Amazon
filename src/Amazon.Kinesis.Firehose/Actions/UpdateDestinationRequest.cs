using System.Runtime.Serialization;

namespace Amazon.Kinesis.Firehose
{
    public class UpdateDestinationRequest
    {
        public string CurrentDeliveryStreamVersionId { get; set; }

        [DataMember(Name = "deliveryStreamName")]
        public string DeliveryStreamName { get; set; }
        
        // TODO
    }
}
