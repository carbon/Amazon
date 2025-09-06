using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Firehose;

public sealed class PutRecordRequest
{
    public PutRecordRequest(string deliveryStreamName, Record record)
    {
        ArgumentException.ThrowIfNullOrEmpty(deliveryStreamName);

        DeliveryStreamName = deliveryStreamName;
        Record = record;
    }

    [JsonPropertyName("DeliveryStreamName")]
    public string DeliveryStreamName { get; }

    [JsonPropertyName("Record")]
    public Record Record { get; }
}