using System.Text.Json.Serialization;

namespace Amazon.Kinesis.Serialization;

[JsonSerializable(typeof(DescribeStreamRequest))]
[JsonSerializable(typeof(DescribeStreamResult))]
[JsonSerializable(typeof(PutRecordsRequest))]
[JsonSerializable(typeof(PutRecordsResult))]
[JsonSerializable(typeof(PutRecordResult))]
[JsonSerializable(typeof(GetRecordsRequest))]
[JsonSerializable(typeof(GetRecordsResult))]
[JsonSerializable(typeof(GetShardIteratorRequest))]
[JsonSerializable(typeof(GetShardIteratorResult))]
[JsonSerializable(typeof(MergeShardsRequest))]
[JsonSerializable(typeof(MergeShardsResult))]
[JsonSerializable(typeof(ErrorResult))]
public partial class KinesisSerializerContext : JsonSerializerContext
{
}