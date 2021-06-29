using System;
using System.Net.Http;

using Amazon.Security;

namespace Amazon.S3
{
    public static class S3Helper
    {
        private const string UnsignedPayload = "UNSIGNED-PAYLOAD";

        public static string GetPresignedUrl(GetPresignedUrlRequest request, IAwsCredential credential)
        {
            return GetPresignedUrl(request, credential, DateTime.UtcNow);
        }

        public static string GetPresignedUrl(GetPresignedUrlRequest request, IAwsCredential credential, DateTime now)
        {
            HttpMethod method = request.Method switch
            {
                "GET"  => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                _      => new HttpMethod(request.Method)
            };

            // TODO: support version querystring

            return SignerV4.GetPresignedUrl(
                credential  : credential, 
                scope       : new CredentialScope(now, request.Region, AwsService.S3),
                date        : now,
                expires     : request.ExpiresIn, 
                method      : method,
                requestUri  : new Uri(request.GetUrl()),
                payloadHash : UnsignedPayload
            );
        }
    }
}