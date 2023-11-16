using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class SendMessageRequest : SqsRequest
{
    public SendMessageRequest(
        string queueUrl,
        string messageBody,
        Dictionary<string, MessageAttributeValue>? messageAttributes = null,
        TimeSpan? delay = null,
        string? messageDeduplicationId = null,
        string? messageGroupId = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(messageBody);

        QueueUrl = queueUrl;
        MessageBody = messageBody;
        MessageAttributes = messageAttributes;
        MessageDeduplicationId = messageDeduplicationId;
        MessageGroupId = messageGroupId;

        if (delay != null)
        {
            DelaySeconds = (int)delay.Value.TotalSeconds;
        }
    }

    [JsonPropertyName("QueueUrl")]
    public string QueueUrl { get; }

    /// <summary>
    /// The number of seconds (0 to 900 - 15 minutes) to delay a specific message. 
    /// Messages with a positive DelaySeconds value become available for processing after the delay time is finished. 
    /// If you don't specify a value, the default value for the queue applies.
    /// </summary>
    [JsonPropertyName("DelaySeconds")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    [Range(0, 900)]
    public int? DelaySeconds { get; }

    [JsonPropertyName("MessageAttributes")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Dictionary<string, MessageAttributeValue>? MessageAttributes { get; }

    [JsonPropertyName("MessageBody")]
    public string MessageBody { get; }

    [JsonPropertyName("MessageDeduplicationId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MessageDeduplicationId { get; }

    // Required for FIFO queues
    [JsonPropertyName("MessageGroupId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MessageGroupId { get; }
}