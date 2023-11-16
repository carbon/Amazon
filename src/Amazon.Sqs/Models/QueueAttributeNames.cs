namespace Amazon.Sqs;

public sealed class QueueAttributeNames
{
    public const string DelaySeconds = nameof(DelaySeconds);
    public const string MaximumMessageSize = nameof(MaximumMessageSize);
    public const string MessageRetentionPeriod = nameof(MessageRetentionPeriod);
    public const string Policy = nameof(Policy);
    public const string ReceiveMessageWaitTimeSeconds = nameof(ReceiveMessageWaitTimeSeconds);
    public const string VisibilityTimeout = nameof(VisibilityTimeout);
    public const string KmsMasterKeyId = nameof(KmsMasterKeyId);
    public const string KmsDataKeyReusePeriodSeconds = nameof(KmsDataKeyReusePeriodSeconds);
    public const string SqsManagedSseEnabled = nameof(SqsManagedSseEnabled);
}