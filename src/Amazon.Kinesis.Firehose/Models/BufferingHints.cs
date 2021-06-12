namespace Amazon.Kinesis.Firehose
{
    public sealed class BufferingHints
    {
        public int IntervalInSeconds { get; init; }

        public int SizeInMBs { get; init; }
    }
}