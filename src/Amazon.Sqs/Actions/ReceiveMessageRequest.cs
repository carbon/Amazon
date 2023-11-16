using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ReceiveMessageRequest : SqsRequest
{
    /// <param name="visibilityTimeout">The time to hide the message from other workers</param>
    /// <param name="waitTime">The maximum amount of time to wait for queue items</param>
    public ReceiveMessageRequest(
        string queueUrl,
        int maxNumberOfMessages = 1,
        string[]? attributeNames = null,
        string[]? messageAttributeNames = null,
        TimeSpan? visibilityTimeout = null,
        TimeSpan? waitTime = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);

        if (maxNumberOfMessages is <= 0 or > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(maxNumberOfMessages), maxNumberOfMessages, "Must be between 1 and 10");
        }

        if (visibilityTimeout.HasValue && visibilityTimeout.Value.TotalHours > 12)
        {
            throw new ArgumentException("Must be less than 12 hours", nameof(visibilityTimeout));
        }

        if (waitTime.HasValue && waitTime.Value.TotalSeconds > 20)
        {
            throw new ArgumentException("Must be less than 20 seconds", nameof(waitTime));
        }

        QueueUrl = queueUrl;
        AttributeNames = attributeNames;
        MaxNumberOfMessages = maxNumberOfMessages;
        MessageAttributeNames = messageAttributeNames;

        if (visibilityTimeout.HasValue)
        {
            VisibilityTimeout = (int)visibilityTimeout.Value.TotalSeconds;
        }

        if (waitTime.HasValue)
        {
            WaitTimeSeconds = (int)waitTime.Value.TotalSeconds;
        }
    }

    [JsonPropertyName("AttributeNames")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? AttributeNames { get; init; }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }

    [JsonPropertyName("MaxNumberOfMessages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? MaxNumberOfMessages { get; init; }

    [JsonPropertyName("MessageAttributeNames")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string[]? MessageAttributeNames { get; init; }

    [JsonPropertyName("ReceiveRequestAttemptId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? ReceiveRequestAttemptId { get; init; }

    [JsonPropertyName("VisibilityTimeout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? VisibilityTimeout { get; init; }

    [JsonPropertyName("WaitTimeSeconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int? WaitTimeSeconds { get; init; }
}

// https://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ReceiveMessage.html