#nullable disable

namespace Amazon.Ssm;

public sealed class LoggingInfo
{
    public string S3BucketName { get; set; }

    public string S3KeyPrefix { get; set; }

    public string S3Region { get; set; }
}
