namespace Amazon.Kinesis
{
	using System.Collections.Generic;

	public class PutRecordsResult
	{
		public int FailedRecordCount { get; set; }

		public List<RecordResult> Records { get; set; }
	}


	public class RecordResult
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

/*
 {
	"ErrorCode": "ProvisionedThroughputExceededException",
    "ErrorMessage": "Rate exceeded for shard shardId-000000000001 in stream exampleStreamName under account 111111111111."
 },
 {
	"ErrorCode": "InternalFailure",
    "ErrorMessage": "Internal service failure."
 }
*/