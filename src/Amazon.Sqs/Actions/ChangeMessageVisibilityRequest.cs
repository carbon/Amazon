using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class ChangeMessageVisibilityRequest : SqsRequest
{
    public ChangeMessageVisibilityRequest(
        string queueUrl,
        string receiptHandle,
        TimeSpan visibilityTimeout)
        : this(queueUrl, receiptHandle, (int)visibilityTimeout.TotalSeconds) { }

    public ChangeMessageVisibilityRequest(
       string queueUrl,
       string receiptHandle,
       int visibilityTimeout)
    {
        ArgumentException.ThrowIfNullOrEmpty(queueUrl);
        ArgumentException.ThrowIfNullOrEmpty(receiptHandle);

        if (visibilityTimeout < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(visibilityTimeout), visibilityTimeout, "Must be greater than 0");
        }

        if (visibilityTimeout > 43_200)
        {
            throw new ArgumentOutOfRangeException(nameof(visibilityTimeout), visibilityTimeout, "Must be less than 12 hours");
        }

        QueueUrl = queueUrl;
        ReceiptHandle = receiptHandle;
        VisibilityTimeout = visibilityTimeout;
    }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }

    [JsonPropertyName("ReceiptHandle")]
    public string ReceiptHandle { get; }
    
    [JsonPropertyName("VisibilityTimeout")] // in seconds
    public int VisibilityTimeout { get; }
}