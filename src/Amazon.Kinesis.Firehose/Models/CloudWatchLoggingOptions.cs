#nullable disable

namespace Amazon.Kinesis.Firehose;

public sealed class CloudWatchLoggingOptions
{
    public bool Enabled { get; init; }

    public string LogGroupName { get; init; }

    public string LogStreamName { get; init; }
}