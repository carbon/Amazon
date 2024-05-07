using System.Globalization;
using System.Net.Http;
using System.Text;

using Amazon.S3.Helpers;

using Carbon.Storage;

namespace Amazon.S3;

public sealed class CompleteMultipartUploadRequest : S3Request
{
    public CompleteMultipartUploadRequest(string host, IUpload upload, IUploadBlock[] parts)
        : this(host, upload.BucketName, upload.ObjectName, upload.UploadId, parts) { }

    public CompleteMultipartUploadRequest(string host, string bucketName, string key, string uploadId, IUploadBlock[] parts)
        : base(HttpMethod.Post, host, bucketName, objectName: $"{key}?uploadId={uploadId}")
    {
        CompletionOption = HttpCompletionOption.ResponseContentRead;

        Content = new StringContent(
            new CompleteMultipartUpload(parts).ToXmlString(),
            Encoding.UTF8,
            "text/xml"
        );
    }
}

public sealed class CompleteMultipartUpload(IUploadBlock[] parts)
{
    public IUploadBlock[] Parts { get; } = parts ?? throw new ArgumentNullException(nameof(parts));

    public string ToXmlString()
    {
        var sb = new XmlStringBuilder(true);

        sb.WriteTagStart("CompleteMultipartUpload");
        {
            foreach (var part in Parts)
            {
                sb.WriteTagStart("Part");
                {
                    sb.WriteTag("PartNumber", part.Number.ToString(CultureInfo.InvariantCulture));
                    sb.WriteTag("ETag", part.BlockId);
                }
                sb.WriteTagEnd("Part");
            }
        }
        sb.WriteTagEnd("CompleteMultipartUpload");

        return sb.ToString();
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
