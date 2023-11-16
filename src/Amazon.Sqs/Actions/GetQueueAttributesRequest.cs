using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class GetQueueAttributesRequest(
    string queueUrl,
    string[] attributeNames) : SqsRequest
{
    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; } = queueUrl;

    [JsonPropertyName("AttributeNames")]
    public string[] AttributeNames { get; } = attributeNames;
}
