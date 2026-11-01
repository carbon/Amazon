namespace Amazon.Sqs;

public sealed class QueueAttributeNames
{
    public const string All = nameof(All);
    public const string ApproximateNumberOfMessages = nameof(ApproximateNumberOfMessages);
    public const string ApproximateNumberOfMessagesDelayed = nameof(ApproximateNumberOfMessagesDelayed);
    public const string ApproximateNumberOfMessagesNotVisible = nameof(ApproximateNumberOfMessagesNotVisible);
    public const string ContentBasedDeduplication = nameof(ContentBasedDeduplication);
    public const string CreatedTimestamp = nameof(CreatedTimestamp);
    public const string DeduplicationScope = nameof(DeduplicationScope);
    public const string DelaySeconds = nameof(DelaySeconds);
    public const string FifoQueue = nameof(FifoQueue);
    public const string FifoThroughputLimit = nameof(FifoThroughputLimit);
    public const string LastModifiedTimestamp = nameof(LastModifiedTimestamp);
    public const string QueueArn = nameof(QueueArn);
    public const string RedriveAllowPolicy = nameof(RedriveAllowPolicy);
    public const string RedrivePolicy = nameof(RedrivePolicy);
    public const string MaximumMessageSize = nameof(MaximumMessageSize);
    public const string MessageRetentionPeriod = nameof(MessageRetentionPeriod);
    public const string Policy = nameof(Policy);
    public const string ReceiveMessageWaitTimeSeconds = nameof(ReceiveMessageWaitTimeSeconds);
    public const string VisibilityTimeout = nameof(VisibilityTimeout);
    public const string KmsMasterKeyId = nameof(KmsMasterKeyId);
    public const string KmsDataKeyReusePeriodSeconds = nameof(KmsDataKeyReusePeriodSeconds);
    public const string SqsManagedSseEnabled = nameof(SqsManagedSseEnabled);
}