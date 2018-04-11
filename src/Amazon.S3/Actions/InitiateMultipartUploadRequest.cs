using System;
using System.Net.Http;

namespace Amazon.S3
{
    public sealed class InitiateMultipartUploadRequest : S3Request
    {
        public InitiateMultipartUploadRequest(string host, string bucketName, string key)
            : base(HttpMethod.Post, host, bucketName, key + "?uploads")
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

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
