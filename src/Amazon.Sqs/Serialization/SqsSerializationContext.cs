using System.Text.Json.Serialization;

namespace Amazon.Sqs.Serialization;

[JsonSerializable(typeof(ChangeMessageVisibilityRequest))]
[JsonSerializable(typeof(ChangeMessageVisibilityBatchRequest))]
[JsonSerializable(typeof(ChangeMessageVisibilityBatchResult))]
[JsonSerializable(typeof(CreateQueueRequest))]
[JsonSerializable(typeof(CreateQueueResult))]
[JsonSerializable(typeof(DeleteMessageBatchRequest))]
[JsonSerializable(typeof(DeleteMessageBatchResult))]
[JsonSerializable(typeof(DeleteMessageRequest))]
[JsonSerializable(typeof(DeleteQueueRequest))]
[JsonSerializable(typeof(GetQueueAttributesRequest))]
[JsonSerializable(typeof(GetQueueAttributesResult))]
[JsonSerializable(typeof(PurgeQueueRequest))]
[JsonSerializable(typeof(ReceiveMessageRequest))]
[JsonSerializable(typeof(ReceiveMessageResult))]
[JsonSerializable(typeof(SendMessageRequest))]
[JsonSerializable(typeof(SendMessageResult))]
[JsonSerializable(typeof(SendMessageBatchRequest))]
[JsonSerializable(typeof(SendMessageBatchResult))]
[JsonSerializable(typeof(ErrorResult))]
public partial class SqsSerializerContext : JsonSerializerContext
{
}