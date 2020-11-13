using System.Net;

using Amazon.Exceptions;
using Amazon.Scheduling;

namespace Amazon.Kinesis
{
    public sealed class KinesisException : AwsException, IException
	{
		private readonly ErrorResult error;

		public KinesisException(ErrorResult error, HttpStatusCode statusCode)
			: base(error.Type ?? error.Text, statusCode)
		{
			this.error = error;
		}

        public string Type => error.Type;

		public bool IsTransient => error.Type is "ProvisionedThroughputExceededException" or "InternalFailure";
	}
}

/*
 {
	"ErrorCode": "ProvisionedThroughputExceededException",
    "ErrorMessage": "Rate exceeded for shard shardId-000000000001 in stream exampleStreamName under account 111111111111."
 },

*/