using System.Net;

using Amazon.Exceptions;
using Amazon.Scheduling;

namespace Amazon.Kinesis;

public sealed class KinesisException(ErrorResult error, HttpStatusCode statusCode)
    : AwsException(error.Type ?? error.Text, statusCode), IException
{
    private readonly ErrorResult _error = error;

    public string Type => _error.Type;

    public bool IsTransient => _error.Type is "ProvisionedThroughputExceededException" or "InternalFailure";
}

/*
 {
	"ErrorCode": "ProvisionedThroughputExceededException",
    "ErrorMessage": "Rate exceeded for shard shardId-000000000001 in stream exampleStreamName under account 111111111111."
 }
*/