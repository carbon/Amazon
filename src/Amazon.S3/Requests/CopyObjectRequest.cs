namespace Amazon.S3
{
    using System.Net.Http;

    public class CopyObjectRequest : S3Request
    {
        public CopyObjectRequest(AwsRegion region, S3ObjectLocation source, S3ObjectLocation target)
            : base(HttpMethod.Put, region, target.BucketName, target.Key)
        {
            Headers.Add("x-amz-copy-source", $"/{source.BucketName}/{source.Key}");

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }
    }
}