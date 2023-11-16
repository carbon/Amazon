namespace Amazon.Sqs;

public static class MessageAttributeNames
{
    public const string All                              = nameof(All);
    public const string ApproximateFirstReceiveTimestamp = nameof(ApproximateFirstReceiveTimestamp);
    public const string ApproximateReceiveCount          = nameof(ApproximateReceiveCount);
    public const string AWSTraceHeader                   = nameof(AWSTraceHeader);
    public const string SenderId                         = nameof(SenderId);
    public const string SentTimestamp                    = nameof(SentTimestamp);
    public const string MessageDeduplicationId           = nameof(MessageDeduplicationId);
    public const string MessageGroupId                   = nameof(MessageGroupId);
    public const string SequenceNumber                   = nameof(SequenceNumber);
}