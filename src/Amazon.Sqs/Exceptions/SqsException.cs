using Amazon.Scheduling;

using Carbon.Messaging;

namespace Amazon.Sqs.Exceptions;

public sealed class SqsException(ErrorResult error) 
    : QueueException(error.Message), IException
{
    public ErrorResult Error { get; } = error;

    public string Type => Error.Type;

    public bool IsTransient => false;
}