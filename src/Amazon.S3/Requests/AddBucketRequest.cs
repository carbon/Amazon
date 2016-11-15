namespace Amazon.S3
{
    using System.Net.Http;

    public class AddBucketRequest : S3Request
    {
        public AddBucketRequest(AwsRegion region, string bucketName)
            : base(HttpMethod.Put, region, bucketName, null)
        {

        }
    }
}