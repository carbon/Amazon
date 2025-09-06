using System.Text.Json.Serialization;

using Amazon.Kinesis.Firehose;

namespace Amazon.Kinesis.Serialization;

[JsonSerializable(typeof(CreateDeliveryStreamRequest))]
[JsonSerializable(typeof(CreateDeliveryStreamResult))]
[JsonSerializable(typeof(DeleteDeliveryStreamRequest))]
[JsonSerializable(typeof(DeleteDeliveryStreamResult))]
[JsonSerializable(typeof(DescribeDeliveryStreamRequest))]
[JsonSerializable(typeof(DescribeDeliveryStreamResult))]
[JsonSerializable(typeof(ListDeliveryStreamsRequest))]
[JsonSerializable(typeof(ListDeliveryStreamsResult))]
[JsonSerializable(typeof(PutRecordRequest))]
[JsonSerializable(typeof(PutRecordResult))]
[JsonSerializable(typeof(PutRecordBatchRequest))]
[JsonSerializable(typeof(PutRecordBatchResult))]
public partial class KinesisFirehoseSerializerContext : JsonSerializerContext
{
}