using System.Net.Http;

namespace Amazon.S3
{
    public class AbortMultipartUploadRequest : S3Request
    {
        // /ObjectName?uploadId=UploadId 
        public AbortMultipartUploadRequest(string host, string bucketName, string key, string uploadId)
            : base(HttpMethod.Delete, host, bucketName, key + "?uploadId=" + uploadId)
        { }
    }
}