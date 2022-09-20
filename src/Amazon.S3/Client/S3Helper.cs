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

    public static string GetPresignedUrl(GetPresignedUrlRequest request, IAwsCredential credential, DateTime utcNow)
    {
        // TODO: support version querystring

        return SignerV4.GetPresignedUrl(
            credential  : credential, 
            scope       : new CredentialScope(DateOnly.FromDateTime(utcNow), request.Region, AwsService.S3),
            date        : utcNow,
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