#nullable disable

using System.Collections.Generic;

namespace Amazon.Kinesis
{
	public sealed class PutRecordsResult : KinesisResponse
	{
		public int FailedRecordCount { get; set; }

		public List<RecordResult> Records { get; set; }
	}

	public sealed class RecordResult
	{
		public string SequenceNumber { get; set; }

		public string ShardId { get; set; }

		public string ErrorCode { get; set; }

		public string ErrorMessage { get; set; }
	}
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