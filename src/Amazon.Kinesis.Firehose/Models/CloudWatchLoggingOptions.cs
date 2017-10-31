namespace Amazon.Kinesis.Firehose
{
    public class CloudWatchLoggingOptions
    {
        public bool Enabled { get; set; }
        
        public string LogGroupName { get; set; }

        public string LogStreamName { get; set; }
    }
    
}
