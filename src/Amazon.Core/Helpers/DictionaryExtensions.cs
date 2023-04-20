using System.Text;
using System.Text.Encodings.Web;

namespace Amazon.Helpers;

public static class DictionaryExtensions
{
    public static string ToPostData(this List<KeyValuePair<string, string>> nvc)
    {
        if (nvc is null || nvc.Count is 0)
        {
            return string.Empty;
        }

        return ToPostData(nvc, null);
    }

    public static string ToQueryString(this List<KeyValuePair<string, string>> nvc)
    {
        if (nvc is null || nvc.Count is 0)
        {
            return string.Empty;
        }

        return nvc.ToPostData(prefix: '?');
    }

    private static string ToPostData(this List<KeyValuePair<string, string>> nvc, char? prefix)
    {
        var sb = new ValueStringBuilder(nvc.Count * 24);

        if (prefix is not null)
        {
            sb.Append(prefix.Value);
        }

        foreach (var (key, value) in nvc)
        {
            if (string.IsNullOrEmpty(key)) continue;

            if (sb.Length > 1)
            {
                sb.Append('&');
            }

            sb.Append(key);
            sb.Append('=');
            sb.Append(UrlEncoder.Default.Encode(value));
        }

        return sb.ToString();
    }
}
