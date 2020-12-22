using System.Linq;
using System.Net.Http.Headers;

namespace Amazon.S3
{
    internal static class HttpResponseHeaderExtensions
    {
        public static string? GetValueOrDefault(this HttpResponseHeaders headers, string name)
        {
            return headers.TryGetValues(name, out var values) ? values.FirstOrDefault() : null;
        }
    }
}