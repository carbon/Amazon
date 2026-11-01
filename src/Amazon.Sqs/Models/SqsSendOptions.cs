namespace Amazon.Sqs;

/// <summary>
/// Per-message options for the queue classes
/// </summary>
public sealed class SqsSendOptions
{
    /// <summary>
    /// On a FIFO queue, the ordered group of the message (required).
    /// On a standard queue, the tenant of the message, which enables fair queues.
    /// </summary>
    public string? MessageGroupId { get; init; }

    /// <summary>
    /// FIFO queues only. Required unless the queue has ContentBasedDeduplication.
    /// </summary>
    public string? MessageDeduplicationId { get; init; }

    /// <summary>
    /// Standard queues only. FIFO queues only support a delay on the queue.
    /// </summary>
    public TimeSpan? Delay { get; init; }
}
