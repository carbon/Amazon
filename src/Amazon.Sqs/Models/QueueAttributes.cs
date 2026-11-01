using System.Text.Json.Serialization;

namespace Amazon.Sqs;

public sealed class QueueAttributes
{
    /// <summary>
    /// The length of time, in seconds, for which the delivery of all messages in the queue is delayed. 
    /// Valid values: An integer from 0 to 900 seconds (15 minutes). Default: 0.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public int? DelaySeconds { get; set; }

    /// <summary>
    /// MaximumMessageSize – The limit of how many bytes a message can contain before Amazon SQS rejects it. 
    /// Valid values: An integer from 1,024 bytes (1 KiB) to 1,048,576 bytes (1 MiB). Default: 1,048,576 (1 MiB).
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public int? MaximumMessageSize { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? MessageRetentionPeriod { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Policy { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ReceiveMessageWaitTimeSeconds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonNumberHandling(JsonNumberHandling.WriteAsString)]
    public int? VisibilityTimeout { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? KmsMasterKeyId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? KmsDataKeyReusePeriodSeconds { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SqsManagedSseEnabled { get; set; }

    /// <summary>
    /// A JSON object with deadLetterTargetArn and maxReceiveCount
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RedrivePolicy { get; set; }

    /// <summary>
    /// A JSON object with redrivePermission and sourceQueueArns
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? RedriveAllowPolicy { get; set; }

    /// <summary>
    /// "true" for a FIFO queue, whose name must end with .fifo. Can only be set when creating the queue.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FifoQueue { get; set; }

    /// <summary>
    /// "true" to derive a MessageDeduplicationId from the message body. FIFO queues only.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ContentBasedDeduplication { get; set; }

    /// <summary>
    /// messageGroup or queue. Set to messageGroup, with a FifoThroughputLimit of perMessageGroupId, for high throughput FIFO.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DeduplicationScope { get; set; }

    /// <summary>
    /// perQueue or perMessageGroupId
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? FifoThroughputLimit { get; set; }
}
