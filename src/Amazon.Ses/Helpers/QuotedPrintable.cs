using System;
using System.Net.Mail;
using System.Text;

namespace Amazon.Ses
{
    public static class QuotedPrintable
    {
        public static string Encode(string text)
        {
            // RFC 2047.

            string encoded = EncodeContent(text);

            return (encoded != text)
                ? $"=?utf-8?Q?{encoded}?="
                : text;
        }

        public static string Decode(string text)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (text.IndexOf(' ') > 0)
            {
                text = text.Replace(" ", string.Empty);
            }
         

            using var a = Attachment.CreateAttachmentFromString(string.Empty, text);

            return a.Name;
        }

        // Based on https://stackoverflow.com/questions/11793734/code-for-encode-decode-quotedprintable

        private static string EncodeContent(string value)
        {
            if (string.IsNullOrEmpty(value)) return value;

            var sb = new StringBuilder();

            byte[] bytes = Encoding.UTF8.GetBytes(value);

            foreach (byte v in bytes)
            {
                // The following are not required to be encoded:
                // - Tab (ASCII 9)
                // - Space (ASCII 32)
                // - Characters 33 to 126, except for the equal sign (61).

                if ((v == 9) || ((v >= 32) && (v <= 60)) || ((v >= 62) && (v <= 126)))
                {
                    sb.Append(Convert.ToChar(v));
                }
                else
                {
                    sb.Append('=');

                    sb.Append(v.ToString("X2"));
                }
            }

            char lastChar = sb[sb.Length - 1];

            if (char.IsWhiteSpace(lastChar))
            {
                sb.Remove(sb.Length - 1, 1);
                sb.Append('=');
                sb.Append(((int)lastChar).ToString("X2"));
            }

            return sb.ToString();
        }
    }
}