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

            var urlBuilder = StringBuilderCache.Aquire()
                .Append("https://")
                .Append(request.Host)
                .Append('/')
                .Append(request.BucketName)
                .Append('/')
                .Append(request.Key);

            // TODO: support version querystring

            var message = new HttpRequestMessage(new HttpMethod(request.Method), StringBuilderCache.ExtractAndRelease(urlBuilder));

            SignerV4.Default.Presign(credential, scope, now, request.ExpiresIn, message, "UNSIGNED-PAYLOAD");

            string signedUrl = message.RequestUri.ToString();

            return signedUrl;
        }
    }
}