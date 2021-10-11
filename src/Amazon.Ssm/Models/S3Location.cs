#nullable disable

namespace Amazon.Ssm;

public sealed class S3Location
{
    public string OutputS3BucketName { get; set; }

    public string OutputS3KeyPrefix { get; set; }

    public string OutputS3Region { get; set; }
}