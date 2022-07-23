#nullable disable

namespace Amazon.Kinesis;

public sealed class PutRecordsResult : KinesisResponse
{
    public int FailedRecordCount { get; init; }

    public List<RecordResult> Records { get; init; }
}

public sealed class RecordResult
{
    public string SequenceNumber { get; init; }

    public string ShardId { get; init; }

    public string ErrorCode { get; init; }

    public string ErrorMessage { get; init; }
}

/*
{
    "FailedRecordCount": "number",
    "Records": [
        {
            "ErrorCode": "string",
            "ErrorMessage": "string",
            "SequenceNumber": "string",
            "ShardId": "string"
        }
    ]
}
*/