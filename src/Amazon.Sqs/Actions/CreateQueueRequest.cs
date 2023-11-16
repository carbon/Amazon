using System.Globalization;
using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class CreateQueueRequest : SqsRequest
{
    public CreateQueueRequest(string queueName, TimeSpan? defaultVisibilityTimeout = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueName);

        QueueName = queueName;

        if (defaultVisibilityTimeout.HasValue)
        {
            var visibilityTimeoutSeconds = (int)defaultVisibilityTimeout.Value.TotalSeconds;

            Attributes = new() {
                VisibilityTimeout = visibilityTimeoutSeconds
            };
        }
    }

    public CreateQueueRequest(string queueName, QueueAttributes attributes)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueName);

        QueueName = queueName;
        Attributes = attributes;
    }

    [JsonPropertyName("QueueName")]
    public string QueueName { get; }

    [JsonPropertyName("Attributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public QueueAttributes? Attributes { get; }

    [JsonPropertyName("tags")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, string>? Tags { get; set; }
}
