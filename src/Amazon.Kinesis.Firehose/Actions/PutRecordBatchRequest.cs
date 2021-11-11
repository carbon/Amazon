using System;

namespace Amazon.Kinesis.Firehose;

public class PutRecordBatchRequest
{
    public PutRecordBatchRequest(string deliveryStreamName, params Record[] records)
    {
        ArgumentNullException.ThrowIfNull(deliveryStreamName);
        ArgumentNullException.ThrowIfNull(records);

        DeliveryStreamName = deliveryStreamName;
        Records = records;
    }

    public string DeliveryStreamName { get; }

    public Record[] Records { get; }
}
