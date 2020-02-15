#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose
{
    public sealed class CreateDeliveryStreamRequest
    {
        public string DeliveryStreamName { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DeliveryStreamType DeliveryStreamType { get; set; }

        public ExtendedS3DestinationConfiguration ExtendedS3DestinationConfiguration { get; set; }
    }
}