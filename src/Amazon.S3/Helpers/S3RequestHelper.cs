using System.Net.Http;
using System.Net.Http.Headers;

namespace Amazon.S3.Helpers;

internal static class S3RequestExtensions
{
    public static void UpdateHeaders(this S3Request request, IReadOnlyDictionary<string, string> headers)
    {
        if (headers is null) return;

        foreach (var item in headers)
        {
            switch (item.Key)
            {
                case "Content-Encoding":
                    request.Content!.Headers.ContentEncoding.Clear();
                    request.Content!.Headers.ContentEncoding.Add(item.Value);
                    break;
                case "Content-Type":
                    request.Content ??= new ByteArrayContent(Array.Empty<byte>());
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(item.Value);

                    break;

                // Skip list...
                case "Accept-Ranges":
                case "Content-Length":
                case "Date":
                case "ETag":
                case "Server":
                case "Last-Modified":
                case "x-amz-id-2":
                case "x-amz-expiration":
                case "x-amz-request-id2":
                case "x-amz-request-id":
                    break;

                default:
                    request.Headers.Add(item.Key, item.Value);

                    break;
            }
        }
    }
}