namespace Amazon.Ses;

public sealed class S3Action
{
    public S3Action(string bucketName, string objectKeyPrefix, string? kmsKeyArn, string? topicArn)
    {
        ArgumentNullException.ThrowIfNull(bucketName);

        BucketName = bucketName;
        ObjectKeyPrefix = objectKeyPrefix;
        KmsKeyArn = kmsKeyArn;
        TopicArn = topicArn;
    }

    public string BucketName { get; }

    public string ObjectKeyPrefix { get; }

    /// <summary>
    /// To use the default master key, provide an ARN in the form of arn:aws:kms:{region}:{accountId}:alias/aws/ses
    /// </summary>
    public string? KmsKeyArn { get; }

    /// <summary>
    /// The ARN of the Amazon SNS topic to notify when the message is saved to the Amazon S3 bucket. 
    /// </summary>
    public string? TopicArn { get; }
}