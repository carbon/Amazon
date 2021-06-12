#nullable disable

namespace Amazon.Kinesis.Firehose
{
    public class PutRecordBatchResult
    {
        public int FailedPutCount { get; init; }

        public RequestResponse[] RequestResponses { get; init; }
    }
}