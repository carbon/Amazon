using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

using Amazon.S3.Helpers;

namespace Amazon.S3
{
    public sealed class InitiateMultipartUploadRequest : S3Request
    {
        public InitiateMultipartUploadRequest(string host, string bucketName, string key)
            : base(HttpMethod.Post, host, bucketName, objectName: key, actionName: S3ActionName.Uploads)
        {
            if (key is null) throw new ArgumentNullException(nameof(key));

            CompletionOption = HttpCompletionOption.ResponseContentRead;

            Content = new ByteArrayContent(Array.Empty<byte>());
        }

        public InitiateMultipartUploadRequest(
            string host,
            string bucketName, 
            string key,
            IReadOnlyDictionary<string, string> properties)
           : this(host, bucketName, key)
        {
            this.UpdateHeaders(properties);
        }

        public string? ContentType
        {
            get => Content!.Headers.ContentType?.ToString();
            set
            {
                if (value is null)
                {
                    Content!.Headers.ContentType = null;
                }
                else
                {
                    Content!.Headers.ContentType = MediaTypeHeaderValue.Parse(value);
                }
            }
        }       
    }
}

/*
POST /ObjectName?uploads HTTP/1.1
Host: BucketName.s3.amazonaws.com
Date: date
Authorization: signatureValue
*/