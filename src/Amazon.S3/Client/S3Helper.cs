using System.Net.Http;

using Amazon.Security;

namespace Amazon.S3;

public static class S3Helper
{
    private const string UnsignedPayload = "UNSIGNED-PAYLOAD";

    public static string GetPresignedUrl(GetPresignedUrlRequest request, IAwsCredential credential)
    {
        return GetPresignedUrl(request, credential, DateTime.UtcNow);
    }

    public static string GetPresignedUrl(GetPresignedUrlRequest request, IAwsCredential credential, DateTime now)
    {
        // TODO: support version querystring

        return SignerV4.GetPresignedUrl(
            credential  : credential, 
            scope       : new CredentialScope(now, request.Region, AwsService.S3),
            date        : now,
            expires     : request.ExpiresIn, 
            method      : GetHttpMethod(request.Method),
            requestUri  : new Uri(request.GetUrl()),
            payloadHash : UnsignedPayload
        );
    }

    private static HttpMethod GetHttpMethod(string name)
    {
        return name switch
        {
            "GET"  => HttpMethod.Get,
            "POST" => HttpMethod.Post,
            _      => new HttpMethod(name)
        };
    }
}