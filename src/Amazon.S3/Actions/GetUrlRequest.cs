namespace Amazon.S3;

public sealed class GetPresignedUrlRequest
{
    public GetPresignedUrlRequest(
        string method,
        string host,
        AwsRegion region,
        string bucketName,
        string objectKey,
        TimeSpan expiresIn)
    {
        ArgumentNullException.ThrowIfNull(method);
        ArgumentNullException.ThrowIfNull(host);
        ArgumentNullException.ThrowIfNull(bucketName);

        Method = method;
        Host = host;
        Region = region;
        BucketName = bucketName;
        Key = objectKey;
        ExpiresIn = expiresIn;
    }

    public string Method { get; }

    public string Host { get; }

    public string BucketName { get; }

    public AwsRegion Region { get; }

    public string Key { get; }

    public TimeSpan ExpiresIn { get; }

    internal string GetUrl() => $"https://{Host}/{BucketName}/{Key}";
}