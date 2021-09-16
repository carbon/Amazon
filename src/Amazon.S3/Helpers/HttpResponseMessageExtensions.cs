using System.Collections.Generic;
using System.Net.Http;

namespace Amazon.S3
{
    internal static class HttpResponseMessageExtensions
    {
        public static Dictionary<string, string> GetProperties(this HttpResponseMessage response)
        {
#if NET5_0
            var result = new Dictionary<string, string>(16);

            foreach (var header in response.Headers)
            {
                result.Add(header.Key, string.Join(';', header.Value));
            }

            foreach (var header in response.Content.Headers)
            {
                result.Add(header.Key, string.Join(';', header.Value));
            }


#else
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
#endif

            return result;
        }
    }
}