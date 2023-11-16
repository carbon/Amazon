using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Serialization;

[JsonSerializable(typeof(DescribeStreamResult))]
[JsonSerializable(typeof(PutRecordsRequest))]
[JsonSerializable(typeof(PutRecordResult))]
[JsonSerializable(typeof(GetShardIteratorRequest))]
[JsonSerializable(typeof(GetShardIteratorResult))]
[JsonSerializable(typeof(ErrorResult))]
public partial class KinesisSerializerContext : JsonSerializerContext
{
}