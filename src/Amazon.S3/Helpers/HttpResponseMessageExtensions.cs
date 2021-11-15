using System.Net.Http;

namespace Amazon.S3;

internal static class HttpResponseMessageExtensions
{
    public static Dictionary<string, string> GetProperties(this HttpResponseMessage response)
    {
        var baseHeaders = response.Headers.NonValidated;
        var contentHeaders = response.Content.Headers.NonValidated;

        var result = new Dictionary<string, string>(baseHeaders.Count + contentHeaders.Count);

        foreach (var header in baseHeaders)
        {
            result.Add(header.Key, header.Value.ToString());
        }

        foreach (var header in contentHeaders)
        {
            result.Add(header.Key, header.Value.ToString());
        }

        return result;
    }
}
