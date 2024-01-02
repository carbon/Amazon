using Amazon.Kinesis.Actions;

namespace Amazon.Kinesis;

public sealed class PutRecordsRequest : KinesisRequest
{
    public PutRecordsRequest(string streamName, PutRecordsRequestEntry[] records)
    {
        ArgumentException.ThrowIfNullOrEmpty(streamName);
        ArgumentNullException.ThrowIfNull(records);

        StreamName = streamName;
        Records = records;
    }

    public string StreamName { get; }

    public PutRecordsRequestEntry[] Records { get; }
}