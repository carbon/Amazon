using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

[JsonConverter(typeof(JsonStringEnumConverter<DeliveryStreamType>))]
public enum DeliveryStreamType
{
    DirectPut             = 1,
    KinesisStreamAsSource = 2
}