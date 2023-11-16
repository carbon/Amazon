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
    public string[]? AttributeNames { get; set; }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }

    [JsonPropertyName("MaxNumberOfMessages")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxNumberOfMessages { get; set; }

    [JsonPropertyName("MessageAttributeNames")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? MessageAttributeNames { get; set; }

    [JsonPropertyName("ReceiveRequestAttemptId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReceiveRequestAttemptId { get; set; }

    [JsonPropertyName("VisibilityTimeout")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? VisibilityTimeout { get; set; }

    [JsonPropertyName("WaitTimeSeconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? WaitTimeSeconds { get; set; }
}

// https://docs.aws.amazon.com/AWSSimpleQueueService/latest/APIReference/API_ReceiveMessage.html