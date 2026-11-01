using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class GetQueueUrlRequest : SqsRequest
{
    public GetQueueUrlRequest(string queueName, string? queueOwnerAWSAccountId = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueName);

        QueueName = queueName;
        QueueOwnerAWSAccountId = queueOwnerAWSAccountId;
    }

    [JsonPropertyName("QueueName")]
    public string QueueName { get; }

    [JsonPropertyName("QueueOwnerAWSAccountId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? QueueOwnerAWSAccountId { get; }
}
