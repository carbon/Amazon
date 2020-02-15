#nullable disable

namespace Amazon.Kinesis.Firehose
{
    public class PutRecordBatchResult
    {
        public int FailedPutCount { get; set; }

        public RequestResponse[] RequestResponses { get; set; }
    }
}