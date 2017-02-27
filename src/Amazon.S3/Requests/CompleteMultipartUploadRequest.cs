using System;
using System.Text;
using System.Net.Http;
using System.Xml.Linq;

namespace Amazon.S3
{
    public class CompleteMultipartUploadRequest : S3Request
    {
        public CompleteMultipartUploadRequest(AwsRegion region, IUpload upload, IUploadBlock[] parts)
            : this(region, upload.BucketName, upload.ObjectName, upload.Id, parts) { }

        public CompleteMultipartUploadRequest(AwsRegion region, string bucketName, string key, string uploadId, IUploadBlock[] parts)
            : base(HttpMethod.Post, region, bucketName, key + "?uploadId=" + uploadId)
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
            #region Preconditions

            if (parts == null)
                throw new ArgumentNullException(nameof(parts));

            #endregion

            Parts = parts;
        }

        public IUploadBlock[] Parts { get; }

        public string ToXmlString()
        {
            var root = new XElement("CompleteMultipartUpload");

            foreach (var part in Parts)
            {
                root.Add(new XElement("Part",
                    new XElement("PartNumber", part.Number),
                    new XElement("ETag", part.ETag)
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
