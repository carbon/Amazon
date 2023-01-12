namespace Amazon.Kinesis.Firehose;

public sealed class PutRecordRequest
{
    public PutRecordRequest(string deliveryStreamName, Record record)
    {
        ArgumentException.ThrowIfNullOrEmpty(deliveryStreamName);

        DeliveryStreamName = deliveryStreamName;
        Record = record;
    }

    public string DeliveryStreamName { get; }

    public Record Record { get; }
}