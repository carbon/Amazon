#nullable disable

namespace Amazon.Kinesis.Firehose;

public sealed class RequestResponse
{
    public string ErrorCode { get; init; }

    public string ErrorMessage { get; init; }

    public string RecordId { get; init; }
}