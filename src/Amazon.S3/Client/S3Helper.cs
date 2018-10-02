using System;
using System.Net.Http;
using System.Text;
using Amazon.Security;

namespace Amazon.S3
{
    public static class S3Helper
    {
        public static string GetPresignedUrl(GetPresignedUrlRequest request, IAwsCredential credential)
        {
            return GetPresignedUrl(request, credential, DateTime.UtcNow);
        }

        public static string GetPresignedUrl(GetPresignedUrlRequest request, IAwsCredential credential, DateTime now)
        {
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

            var r = new HttpRequestMessage(new HttpMethod(request.Method), urlBuilder.ToString());

            SignerV4.Default.Presign(credential, scope, now, request.ExpiresIn, r, "UNSIGNED-PAYLOAD");

            string signedUrl = r.RequestUri.ToString();

            return signedUrl;
        }
    }
}