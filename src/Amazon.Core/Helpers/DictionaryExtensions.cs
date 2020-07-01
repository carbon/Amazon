using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace Amazon.Helpers
{
    public static class DictionaryExtensions
    {
        public static string ToPostData(this Dictionary<string, string> nvc)
        {
            if (nvc is null || nvc.Count == 0)
            {
                return string.Empty;
            }

            return ToPostData(nvc, null);
        }

        public static string ToQueryString(this Dictionary<string, string> nvc)
        {
            if (nvc is null || nvc.Count == 0)
            {
                return string.Empty;
            }

            return nvc.ToPostData(prefix: "?");
        }

        private static string ToPostData(this Dictionary<string, string> nvc, string? prefix)
        {
            var sb = new StringBuilder();

            if (prefix != null)
            {
                sb.Append(prefix);
            }

            foreach (string key in nvc.Keys)
            {
                if (string.IsNullOrEmpty(key)) continue;

                if (sb.Length > 1)
                {
                    sb.Append('&');
                }

                sb.Append(key);
                sb.Append('=');
                sb.Append(UrlEncoder.Default.Encode(nvc[key]));
            }

            return sb.ToString();
        }
    }
}