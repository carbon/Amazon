using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum DeliveryStreamType
    {
        DirectPut             = 1,
        KinesisStreamAsSource = 2
    }   
}