#nullable disable

using System.Text.Json.Serialization;

namespace Amazon.Kinesis;

public sealed class ErrorResult
{
    public string Type { get; set; }

    public string Message { get; set; }

    [JsonIgnore]
    public string Text { get; set; }
}

/*
{
  "ErrorCode": "ProvisionedThroughputExceededException",
  "ErrorMessage": "Rate exceeded for shard shardId-000000000001 in stream exampleStreamName under account 111111111111."
}
*/

