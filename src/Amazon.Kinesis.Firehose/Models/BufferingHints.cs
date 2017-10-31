namespace Amazon.Kinesis.Firehose
{
    public class BufferingHints
    {
        public int IntervalInSeconds { get; set; }

        public int SizeInMBs { get; set; }
    }
    
}
