using Carbon.Messaging;

namespace Amazon.Sqs.Exceptions;

/// <summary>
/// A batch request returned HTTP 200, but SQS rejected some of its entries
/// </summary>
public sealed class SqsBatchException(BatchResultErrorEntry[] failed)
    : QueueException($"{failed.Length} batch entries failed. First error = {failed[0].Code}: {failed[0].Message}")
{
    public BatchResultErrorEntry[] Failed { get; } = failed;
}
