namespace Amazon.Kinesis.Firehose;

public sealed class PutRecordBatchRequest
{
    public PutRecordBatchRequest(string deliveryStreamName!!, params Record[] records!!)
    {
        DeliveryStreamName = deliveryStreamName;
        Records = records;
    }

    public string DeliveryStreamName { get; }

    public Record[] Records { get; }
}