namespace Amazon.Kinesis
{
    public class PutRecordResult
    {
        public string SequenceNumber { get; set; }

        public string ShardId { get; set; }
    }

    /*
	{
      "SequenceNumber": "string",
	  "ShardId": "string"
	}
	*/
}
