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
    /// Valid values: An integer from 1,024 bytes (1 KiB) to 262,144 bytes (256 KiB). Default: 262,144 (256 KiB).
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
}
