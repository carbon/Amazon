using System;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

using Carbon.Storage;

namespace Amazon.S3
{
    public sealed class CompleteMultipartUploadRequest : S3Request
    {
        public CompleteMultipartUploadRequest(string host, IUpload upload, IUploadBlock[] parts)
            : this(host, upload.BucketName, upload.ObjectName, upload.UploadId, parts) { }

        public CompleteMultipartUploadRequest(string host, string bucketName, string key, string uploadId, IUploadBlock[] parts)
            : base(HttpMethod.Post, host, bucketName, key + "?uploadId=" + uploadId)
        {
            CompletionOption = HttpCompletionOption.ResponseContentRead;

            Content = new StringContent(
                new CompleteMultipartUpload(parts).ToXmlString(),
                Encoding.UTF8,
                "text/xml"
            );
        }
    }

    public class CompleteMultipartUpload
    {
        public CompleteMultipartUpload(IUploadBlock[] parts)
        {
            Parts = parts ?? throw new ArgumentNullException(nameof(parts));
        }

        public IUploadBlock[] Parts { get; }

        public string ToXmlString()
        {
            var root = new XElement("CompleteMultipartUpload");

            foreach (var part in Parts)
            {
                root.Add(new XElement("Part",
                    new XElement("PartNumber", part.Number),
                    new XElement("ETag",       part.BlockId)
                ));
            }

            return root.ToString();
        }
    }
}

/*
POST /ObjectName?uploadId=UploadId HTTP/1.1
Host: BucketName.s3.amazonaws.com
Date: Date
Content-Length: Size
Authorization: Signature

<CompleteMultipartUpload>
  <Part>
    <PartNumber>PartNumber</PartNumber>
    <ETag>ETag</ETag>
  </Part>
  ...
</CompleteMultipartUpload>
*/
