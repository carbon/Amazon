using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;

namespace Amazon.Helpers
{
    public static class DictionaryExtensions
    {
        public static string ToPostData(this Dictionary<string, string> nvc)
        {
            #region Preconditions

            if (nvc == null) throw new ArgumentNullException(nameof(nvc));

            #endregion

            var sb = new StringBuilder();

            foreach (string key in nvc.Keys)
            {
                if (sb.Length > 0)
                {
                    sb.Append("&");
                }

                sb.Append(key);
                sb.Append("=");
                sb.Append(UrlEncoder.Default.Encode(nvc[key]));
            }

            return sb.ToString();
        }

        public static string ToQueryString(this Dictionary<string, string> nvc)
        {
            if (nvc.Count == 0) return "";

            return "?" + nvc.ToPostData();
        }
    }
}
