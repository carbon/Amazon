#nullable disable

namespace Amazon.Ses;

public sealed class S3Action
{
    public string BucketName { get; set; }

    public string ObjectKeyPrefix { get; set; }

#nullable enable

    /// <summary>
    /// To use the default master key, provide an ARN in the form of arn:aws:kms:{region}:{accountId}:alias/aws/ses
    /// </summary>
    public string? KmsKeyArn { get; set; }

    /// <summary>
    /// The ARN of the Amazon SNS topic to notify when the message is saved to the Amazon S3 bucket. 
    /// </summary>
    public string? TopicArn { get; set; }
}