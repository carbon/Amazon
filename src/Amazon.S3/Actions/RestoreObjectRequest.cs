using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3
{
    public class RestoreObjectRequest : S3Request
    {
        public RestoreObjectRequest(string host, string bucketName, string key)
            : base(HttpMethod.Post, host, bucketName, key + "?restore")
        {
            var xmlText = GetXmlString();

            Content = new StringContent(xmlText, Encoding.UTF8, "text/xml");
            Content.Headers.ContentMD5 = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(xmlText));

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }

        public GlacierJobTier Tier { get; set; } = GlacierJobTier.Standard;

        public int Days { get; set; } = 7; // Default to 7

        public string GetXmlString() =>
$@"<RestoreRequest>
  <Days>{Days}</Days>
  <GlacierJobParameters>
    <Tier>{Tier}</Tier>
  </GlacierJobParameters>
</RestoreRequest>";
    }
}

/*
POST /ObjectName?restore&versionId=VersionID HTTP/1.1
Host: BucketName.s3.amazonaws.com
<RestoreRequest xmlns="http://s3.amazonaws.com/doc/2006-3-01">
   <Days>NumberOfDays</Days>
</RestoreRequest> 
*/
