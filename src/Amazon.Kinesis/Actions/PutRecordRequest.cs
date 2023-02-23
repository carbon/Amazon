﻿namespace Amazon.Kinesis;

public sealed class PutRecordsRequest : KinesisRequest
{
    public PutRecordsRequest(string streamName, Record[] records)
    {
        ArgumentException.ThrowIfNullOrEmpty(streamName);
        ArgumentNullException.ThrowIfNull(records);

        StreamName = streamName;
        Records = records;
    }

    public string StreamName { get; }

    public Record[] Records { get; }
}
