#nullable disable

namespace Amazon.Kinesis.Firehose;

public sealed class PutRecordBatchResult
{
    public int FailedPutCount { get; init; }

    public RequestResponse[] RequestResponses { get; init; }
}