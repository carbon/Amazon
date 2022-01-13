using System.Net.Http.Headers;

namespace Amazon.S3;

internal static class HttpResponseHeaderExtensions
{
    public static string? GetValueOrDefault(this HttpResponseHeaders headers, string name)
    {
        return headers.NonValidated.TryGetValues(name, out var values) ? values.ToString() : null;
    }
}