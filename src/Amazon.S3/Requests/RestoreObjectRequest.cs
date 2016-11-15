using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace Amazon.S3
{
    public class RestoreObjectRequest : S3Request
    {
        private readonly int days;

        public RestoreObjectRequest(AwsRegion region, string bucketName, string key, int days)
            : base(HttpMethod.Post, region, bucketName, key + "?restore")
        {
            this.days = days;

            var xmlText = GetXmlString();

            Content = new StringContent(xmlText, Encoding.UTF8, "text/xml");
            Content.Headers.ContentMD5 = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(xmlText));

            CompletionOption = HttpCompletionOption.ResponseContentRead;
        }

        public string GetXmlString() =>
$@"<RestoreRequest>
   <Days>{days}</Days>
</RestoreRequest>";
        
    }
}


/*
POST /ObjectName?restore&versionId=VersionID HTTP/1.1
Host: BucketName.s3.amazonaws.com
Date: date
Authorization: authorization string (see Authenticating Requests (AWS Signature Version 4))
Content-MD5: MD5

<RestoreRequest xmlns="http://s3.amazonaws.com/doc/2006-3-01">
   <Days>NumberOfDays</Days>
</RestoreRequest> 
*/
