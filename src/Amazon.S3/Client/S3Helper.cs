using System;
using System.Net.Http;
using System.Text;
using Amazon.Security;

namespace Amazon.S3
{
    public static class S3Helper
    {        
        public static string GetPresignedUrl(in GetPresignedUrlRequest request, IAwsCredential credential)
        {
            var now = DateTime.UtcNow;

            var scope = new CredentialScope(now, request.Region, AwsService.S3);

            int urlLength = 10 + request.Host.Length + request.BucketName.Length + request.Key.Length;

            var urlBuilder = new StringBuilder(urlLength)
                .Append("https://")
                .Append(request.Host)
                .Append('/')
                .Append(request.BucketName)
                .Append('/')
                .Append(request.Key);

            // TODO: support version querystring

            var r = new HttpRequestMessage(HttpMethod.Get, urlBuilder.ToString());

            SignerV4.Default.Presign(credential, scope, now, request.ExpiresIn, r, "UNSIGNED-PAYLOAD");

            var signedUrl = r.RequestUri.ToString();

            return signedUrl;
        }
    }
}