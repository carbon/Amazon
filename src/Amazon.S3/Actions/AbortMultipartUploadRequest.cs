using System.Net.Http;

namespace Amazon.S3
{
    public class AbortMultipartUploadRequest : S3Request
    {
        // /ObjectName?uploadId=UploadId 
        public AbortMultipartUploadRequest(AwsRegion region, string bucketName, string key, string uploadId)
            : base(HttpMethod.Delete, region, bucketName, key + "?uploadId=" + uploadId)
        { }
    }
}