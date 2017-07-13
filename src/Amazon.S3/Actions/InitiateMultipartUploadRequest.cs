using System.Net.Http;

namespace Amazon.S3
{
    public class InitiateMultipartUploadRequest : S3Request
    {
        public InitiateMultipartUploadRequest(AwsRegion region, string bucketName, string key)
            : base(HttpMethod.Post, region, bucketName, key + "?uploads")
        {
            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }
    }
}

/*
POST /ObjectName?uploads HTTP/1.1
Host: BucketName.s3.amazonaws.com
Date: date
Authorization: signatureValue
*/
